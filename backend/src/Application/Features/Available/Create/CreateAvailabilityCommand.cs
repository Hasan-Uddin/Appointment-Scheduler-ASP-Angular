using Application.Abstractions.Messaging;

namespace Application.Features.Available.Create;

public sealed record CreateAvailabilityCommand(
    Guid UserId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
) : ICommand<Guid>;
