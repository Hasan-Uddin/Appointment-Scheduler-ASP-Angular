using Application.Abstractions.Messaging;
using Application.Features.Availability.AvailableSlots;
using SharedKernel;

namespace Application.Features.Availability.Get;

public sealed record GetAvailableSlotsQuery(Guid EventTypeId, DateTime Date) : IQuery<List<TimeSlotDto>>;
