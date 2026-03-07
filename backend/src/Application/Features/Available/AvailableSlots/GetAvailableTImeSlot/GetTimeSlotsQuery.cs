using Application.Abstractions.Messaging;

namespace Application.Features.Available.AvailableSlots.GetAvailableTImeSlot;

public sealed record GetTimeSlotsQuery(
    Guid EventTypeId,
    DateTime Date
) : IQuery<List<TimeSlotResponse>>;
