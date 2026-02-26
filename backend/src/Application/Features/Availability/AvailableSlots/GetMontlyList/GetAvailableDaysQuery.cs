using Application.Abstractions.Messaging;

namespace Application.Features.Availability.AvailableSlots.GetMontlyList;

public sealed record GetAvailableDaysQuery(
    Guid EventTypeId,
    int Year,
    int Month
) : IQuery<List<AvailableDayResponse>>;
