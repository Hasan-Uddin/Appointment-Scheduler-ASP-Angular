using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Availabilities;
using Domain.Bookings;
using Domain.EventTypes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.AvailableSlots.GetAvailableTImeSlot;

internal sealed class GetTimeSlotsQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetTimeSlotsQuery, List<TimeSlotResponse>>
{
    public async Task<Result<List<TimeSlotResponse>>> Handle(
        GetTimeSlotsQuery query,
        CancellationToken cancellationToken)
    {
        var date = DateTime.SpecifyKind(
            query.Date.Date,
            DateTimeKind.Utc);

        EventType? eventType = await context.EventTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == query.EventTypeId && e.IsActive,
                cancellationToken);

        if (eventType is null)
        {
            return Result.Failure<List<TimeSlotResponse>>("Event type not found.");
        }

        Guid userId = eventType.UserId;
        DayOfWeek dayOfWeek = date.DayOfWeek;

        List<Availability> availabilities = await context.Availabilities
            .AsNoTracking()
            .Where(a =>
                a.UserId == userId &&
                a.DayOfWeek == dayOfWeek &&
                a.IsActive)
            .ToListAsync(cancellationToken);

        if (!availabilities.Any())
        {
            return Result.Success(new List<TimeSlotResponse>());
        }

        DateTime startOfDay = date;
        DateTime endOfDay = startOfDay.AddDays(1);

        List<Booking> bookings = await context.Bookings
            .AsNoTracking()
            .Where(b =>
                b.UserId == userId &&
                b.Status == BookingStatus.Confirmed &&
                b.StartTime < endOfDay &&
                b.EndTime > startOfDay)
            .ToListAsync(cancellationToken);

        var slots = new List<TimeSlotResponse>();
        DateTime utcNow = DateTime.UtcNow;

        foreach (Availability availability in availabilities)
        {
            DateTime availabilityStart =
                startOfDay.Add(availability.StartTime.ToTimeSpan());

            DateTime availabilityEnd =
                startOfDay.Add(availability.EndTime.ToTimeSpan());

            DateTime currentTime = availabilityStart;

            while (currentTime.AddMinutes(eventType.DurationMinutes)
                   <= availabilityEnd)
            {
                DateTime slotEnd =
                    currentTime.AddMinutes(eventType.DurationMinutes);

                bool hasConflict = bookings.Any(b =>
                {
                    DateTime bufferedStart =
                        b.StartTime.AddMinutes(-eventType.BufferMinutes);

                    DateTime bufferedEnd =
                        b.EndTime.AddMinutes(eventType.BufferMinutes);

                    return currentTime < bufferedEnd &&
                           slotEnd > bufferedStart;
                });

                bool isAvailable =
                    currentTime > utcNow && !hasConflict;

                slots.Add(new TimeSlotResponse
                {
                    StartTime = currentTime.ToString("O"), // ISO 8601
                    EndTime = slotEnd.ToString("O"),
                    IsAvailable = isAvailable
                });

                currentTime = currentTime.AddMinutes(
                    eventType.DurationMinutes + eventType.BufferMinutes);
            }
        }

        return Result.Success(
            slots.OrderBy(s => s.StartTime).ToList());
    }
}
