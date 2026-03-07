
namespace Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);

    Task SendBookingConfirmationAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default);

    Task SendBookingCancellationAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default);

    Task SendBookingReminderAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default);

    Task SendBookingRescheduleAsync(
        BookingRescheduleEmailData rescheduleData,
        CancellationToken cancellationToken = default);
}

public class BookingEmailData
{
    public required string GuestName { get; init; }
    public required string GuestEmail { get; init; }
    public required string HostName { get; init; }
    public required string HostEmail { get; init; }
    public required string EventTypeName { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required int DurationMinutes { get; init; }
    public string? Notes { get; init; }
    public string? MeetingLink { get; init; }
    public string? Location { get; init; }
    public string? TimeZone { get; init; } = "UTC";
    public Guid BookingId { get; init; }
}

public sealed class BookingRescheduleEmailData : BookingEmailData
{
    public required DateTime OldStartTime { get; init; }
    public required DateTime OldEndTime { get; init; }
}
