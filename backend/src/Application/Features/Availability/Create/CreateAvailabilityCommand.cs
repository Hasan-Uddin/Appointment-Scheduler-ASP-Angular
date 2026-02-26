using Application.Abstractions.Messaging;

namespace Application.Features.Availability.Create;

public sealed record CreateAvailabilityCommand(
    Guid UserId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
) : ICommand<Guid>;
