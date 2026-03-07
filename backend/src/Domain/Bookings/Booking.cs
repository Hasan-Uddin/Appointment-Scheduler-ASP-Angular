using Domain.EventTypes;
using Domain.Users;
using SharedKernel;

namespace Domain.Bookings;

public class Booking : Entity
{
    public Guid Id { get; private set; }
    public Guid EventTypeId { get; private set; }
    public Guid UserId { get; private set; } // Host
    public string GuestName { get; private set; }
    public string GuestEmail { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? GoogleEventId { get; private set; }
    public string? GuestPhone { get; set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public EventType EventType { get; set; } //Navigation property
    public User User { get; set; } // Navigation property
    public static Booking Create(
        Guid eventTypeId,
        Guid userId,
        string guestName,
        string guestEmail,
        DateTime startTime,
        DateTime endTime,
        string? notes = null)
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            EventTypeId = eventTypeId,
            UserId = userId,
            GuestName = guestName,
            GuestEmail = guestEmail,
            StartTime = startTime,
            EndTime = endTime,
            Status = BookingStatus.Confirmed,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetGoogleEventId(string eventId)
    {
        GoogleEventId = eventId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newStartTime, DateTime newEndTime)
    {
        StartTime = newStartTime;
        EndTime = newEndTime;
        UpdatedAt = DateTime.UtcNow;
    }
}
