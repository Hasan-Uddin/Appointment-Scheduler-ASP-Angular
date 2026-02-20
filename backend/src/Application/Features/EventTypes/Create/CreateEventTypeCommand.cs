using Application.Abstractions.Messaging;

namespace Application.Features.EventTypes.Create;

public sealed class CreateEventTypeCommand : ICommand<Guid>
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int DurationMinutes { get; init; }
    public int BufferMinutes { get; init; }
    public string? Color { get; init; }
}
