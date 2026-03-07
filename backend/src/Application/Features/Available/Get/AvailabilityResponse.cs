
namespace Application.Features.Available.Get;

public sealed class AvailabilityResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
