
namespace Application.Abstractions.Interfaces;

public interface ISlotCalculator
{
    Task<List<TimeSlot>> CalculateAvailableSlots(
        Guid userId,
        DateTime date,
        int durationMinutes,
        int bufferMinutes,
        CancellationToken cancellationToken);

    Task<bool> IsSlotAvailable(
        Guid userId,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken);
}

public class TimeSlot
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
