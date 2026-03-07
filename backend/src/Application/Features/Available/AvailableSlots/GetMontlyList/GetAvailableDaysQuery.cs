using Application.Abstractions.Messaging;

namespace Application.Features.Available.AvailableSlots.GetMontlyList;

public sealed record GetAvailableDaysQuery(
    Guid EventTypeId,
    int Year,
    int Month
) : IQuery<List<AvailableDayResponse>>;
