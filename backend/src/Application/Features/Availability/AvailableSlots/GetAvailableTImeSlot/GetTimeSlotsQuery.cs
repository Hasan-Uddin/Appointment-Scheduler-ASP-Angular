using Application.Abstractions.Messaging;

namespace Application.Features.Availability.AvailableSlots.GetAvailableTImeSlot;

public sealed record GetTimeSlotsQuery(
    Guid EventTypeId,
    DateTime Date
) : IQuery<List<TimeSlotResponse>>;
