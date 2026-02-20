using Domain.Bookings;
using Domain.Users;
using SharedKernel;

namespace Domain.EventTypes;

public class EventType : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string? Description { get; private set; }
    public int DurationMinutes { get; private set; }
    public int BufferMinutes { get; private set; }
    public bool IsActive { get; private set; }
    public string? Color { get; private set; }

    public User User { get; set; }

    private readonly List<Booking> _bookings = new();

    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private EventType() { }

    public static EventType Create(
        Guid userId,
        string name,
        string slug,
        int durationMinutes,
        int bufferMinutes = 0,
        string? description = null,
        string? color = null)
    {
        if (durationMinutes <= 0)
        {
            throw new Exception("Duration must be positive");
        }

        return new EventType
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            Slug = slug,
            Description = description,
            DurationMinutes = durationMinutes,
            BufferMinutes = bufferMinutes,
            Color = color,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description, int durationMinutes, int bufferMinutes, string? color)
    {
        Name = name;
        Description = description;
        DurationMinutes = durationMinutes;
        BufferMinutes = bufferMinutes;
        Color = color;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
}
