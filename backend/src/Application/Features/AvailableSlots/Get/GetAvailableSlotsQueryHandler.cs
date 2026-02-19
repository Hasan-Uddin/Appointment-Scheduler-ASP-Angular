using Application.Abstractions.Data;
using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.EventTypes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.AvailableSlots.Get;

public class GetAvailableSlotsQueryHandler(
        IApplicationDbContext _context,
        ISlotCalculator _slotCalculator) : IQueryHandler<GetAvailableSlotsQuery, List<TimeSlotDto>>
{


    public async Task<Result<List<TimeSlotDto>>> Handle(
        GetAvailableSlotsQuery request,
        CancellationToken cancellationToken)
    {
        EventType? eventType = await _context.EventTypes
            .Include(e => e.User)
                .ThenInclude(u => u.Availabilities)
            .FirstOrDefaultAsync(e => e.Id == request.EventTypeId, cancellationToken);

        if (eventType == null)
        {
            return Result.Failure<List<TimeSlotDto>>("Event type not found");
        }

        List<TimeSlot> slots = await _slotCalculator.CalculateAvailableSlots(
            eventType.UserId,
            request.Date,
            eventType.DurationMinutes,
            eventType.BufferMinutes,
            cancellationToken);

        var slotDtos = slots.Select(s => new TimeSlotDto
        {
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            IsAvailable = s.IsAvailable
        }).ToList();

        return Result<List<TimeSlotDto>>.Success(slotDtos);
    }
}
