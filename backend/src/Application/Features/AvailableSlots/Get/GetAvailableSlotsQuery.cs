using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Features.AvailableSlots.Get;

public sealed record GetAvailableSlotsQuery(Guid EventTypeId, DateTime Date) : IQuery<List<TimeSlotDto>>;
