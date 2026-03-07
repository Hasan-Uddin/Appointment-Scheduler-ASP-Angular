using Domain.Availabilities;
using Domain.Bookings;
using Domain.EventTypes;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string GoogleId { get; set; }
    public string PictureUrl { get; set; }
    public string? GoogleAccessToken { get; set; }
    public string? GoogleRefreshToken { get; set; }
    public string? PasswordHash { get; set; }
    public string? TimeZone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private readonly List<Availability> _availabilities = new();
    public IReadOnlyCollection<Availability> Availabilities => _availabilities.AsReadOnly();

    private readonly List<EventType> _eventTypes = new();
    public IReadOnlyCollection<EventType> EventTypes => _eventTypes.AsReadOnly();

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    public static User Create(string email, string Name, string timeZone = "UTC")
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = Name,
            TimeZone = timeZone,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string name, string timeZone)
    {
        Name = name;
        TimeZone = timeZone;
        UpdatedAt = DateTime.UtcNow;
    }
}

