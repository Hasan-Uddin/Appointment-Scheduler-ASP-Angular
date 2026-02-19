using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.EventTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Features.Bookings.Create;

public sealed class CreateBookingCommandHandler(
    IApplicationDbContext context,
    IGoogleCalendarService calendarService,
    IEmailService emailService,
    ISlotCalculator slotCalculator,
    ILogger<CreateBookingCommandHandler> logger
) : ICommandHandler<CreateBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        EventType? eventType = await context.EventTypes
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == request.EventTypeId, cancellationToken);

        if (eventType is null)
        {
            return Result.Failure<Guid>(EventTypeErrors.NotFound());
        }

        if (!eventType.IsActive)
        {
            return Result.Failure<Guid>(EventTypeErrors.Inactive);
        }

        DateTime endTime = request.StartTime.AddMinutes(eventType.DurationMinutes);

        bool isAvailable = await slotCalculator.IsSlotAvailable(
            eventType.UserId,
            request.StartTime,
            endTime,
            cancellationToken);

        if (!isAvailable)
        {
            return Result.Failure<Guid>(BookingErrors.SlotNotAvailable);
        }

        bool hasConflict = await context.Bookings
            .AnyAsync(b =>
                b.UserId == eventType.UserId &&
                b.Status == BookingStatus.Confirmed && request.StartTime < b.EndTime && endTime > b.StartTime,
                cancellationToken);

        if (hasConflict)
        {
            return Result.Failure<Guid>(BookingErrors.Conflict);
        }

        var booking = Booking.Create(
            request.EventTypeId,
            eventType.UserId,
            request.GuestName,
            request.GuestEmail,
            request.StartTime,
            endTime,
            request.Notes
        );

        context.Bookings.Add(booking);
        await context.SaveChangesAsync(cancellationToken);

        await SyncToGoogleCalendarAsync(
            booking,
            eventType,
            request,
            endTime,
            cancellationToken);

        await SendConfirmationEmailsAsync(
            booking,
            eventType,
            cancellationToken);

        return Result.Success(booking.Id);
    }

    private async Task SyncToGoogleCalendarAsync(
        Booking booking,
        EventType eventType,
        CreateBookingCommand request,
        DateTime endTime,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(eventType.User.GoogleRefreshToken))
        {
            logger.LogWarning(
                "Google Calendar sync skipped - no refresh token for user {UserId}",
                eventType.UserId);
            return;
        }

        try
        {
            var calendarRequest =
                new GoogleCalendarEventRequest
                {
                    Summary = eventType.Name + " with " + request.GuestName,
                    Description = request.Notes,
                    StartTime = request.StartTime,
                    EndTime = endTime,
                    Attendees = new List<string> { request.GuestEmail }
                };

            string googleEventId =
                await calendarService.CreateEventAsync(
                    eventType.User.GoogleRefreshToken,
                    calendarRequest,
                    cancellationToken);

            booking.SetGoogleEventId(googleEventId);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to sync booking {BookingId} to Google Calendar",
                booking.Id);
        }
    }

    private async Task SendConfirmationEmailsAsync(
        Booking booking,
        EventType eventType,
        CancellationToken cancellationToken)
    {
        try
        {
            var emailData =
                new BookingEmailData
                {
                    BookingId = booking.Id,
                    GuestName = booking.GuestName,
                    GuestEmail = booking.GuestEmail,
                    HostName = eventType.User.Name,
                    HostEmail = eventType.User.Email,
                    EventTypeName = eventType.Name,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    DurationMinutes = eventType.DurationMinutes,
                    Notes = booking.Notes,
                    MeetingLink = booking.GoogleEventId != null
                        ? "https://calendar.google.com/calendar/event?eid=" + booking.GoogleEventId
                        : null,
                    TimeZone = eventType.User.TimeZone ?? "UTC"
                };

            await emailService.SendBookingConfirmationAsync(
                emailData,
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to send confirmation emails for booking {BookingId}",
                booking.Id);
        }
    }
}
