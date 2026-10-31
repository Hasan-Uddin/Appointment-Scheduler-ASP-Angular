using Application.Abstractions.Messaging;
using Application.Features.Available.Get;

namespace Application.Features.Available.Create;

public sealed record CreateAvailabilityCommand(
    Guid UserId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
) : ICommand<AvailabilityResponse>;
