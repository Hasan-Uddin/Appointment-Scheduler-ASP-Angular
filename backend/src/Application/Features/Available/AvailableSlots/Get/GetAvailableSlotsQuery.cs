using Application.Abstractions.Messaging;
using Application.Features.Available.AvailableSlots;
using SharedKernel;

namespace Application.Features.Available.AvailableSlots.Get;

public sealed record GetAvailableSlotsQuery(Guid EventTypeId, DateTime Date) : IQuery<List<TimeSlotDto>>;
