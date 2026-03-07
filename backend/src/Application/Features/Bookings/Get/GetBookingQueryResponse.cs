namespace Application.Features.Bookings.Get;

public sealed class GetBookingQueryResponse
{
    public Guid Id { get; init; }
    public Guid EventTypeId { get; init; }
    public string EventTypeName { get; init; } = string.Empty;
    public string GuestName { get; init; } = string.Empty;
    public string GuestEmail { get; init; } = string.Empty;
    public string HostName { get; init; } = string.Empty;
    public string HostEmail { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public int Duration { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public string? GoogleEventId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
