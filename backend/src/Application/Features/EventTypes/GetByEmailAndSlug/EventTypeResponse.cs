
namespace Application.Features.EventTypes.GetByEmailAndSlug;

public sealed class EventTypeResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string HostName { get; set; }
    public string HostEmail { get; set; }
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public int BufferMinutes { get; set; }
    public bool IsActive { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

