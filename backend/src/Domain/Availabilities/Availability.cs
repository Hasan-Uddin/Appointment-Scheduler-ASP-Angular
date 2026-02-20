using SharedKernel;

namespace Domain.Availabilities;

public class Availability : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Availability() { }

    public static Availability Create(
        Guid userId,
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new Exception("Start time must be before end time");
        }

        return new Availability
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new Exception("Start time must be before end time");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    public void Deactivate() => IsActive = false;
}
