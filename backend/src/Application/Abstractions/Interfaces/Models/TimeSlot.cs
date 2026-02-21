
namespace Application.Abstractions.Interfaces.Models;

public sealed class TimeSlot
{
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public bool IsAvailable { get; init; }
    public string? UnavailableReason { get; init; }

    public static TimeSlot Available(DateTime startTime, DateTime endTime)
    {
        return new TimeSlot
        {
            StartTime = startTime,
            EndTime = endTime,
            IsAvailable = true
        };
    }

    public static TimeSlot Unavailable(DateTime startTime, DateTime endTime, string reason)
    {
        return new TimeSlot
        {
            StartTime = startTime,
            EndTime = endTime,
            IsAvailable = false,
            UnavailableReason = reason
        };
    }
}
