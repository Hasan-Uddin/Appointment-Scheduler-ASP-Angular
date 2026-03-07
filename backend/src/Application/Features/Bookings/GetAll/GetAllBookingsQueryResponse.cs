namespace Application.Features.Bookings.GetAll;

public sealed class GetAllBookingsQueryResponse
{
    public Guid Id { get; init; }
    public string EventTypeName { get; init; } = string.Empty;
    public string GuestName { get; init; } = string.Empty;
    public string GuestEmail { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
