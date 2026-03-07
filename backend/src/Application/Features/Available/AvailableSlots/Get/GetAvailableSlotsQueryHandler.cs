using Application.Abstractions.Data;
using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Features.Available.AvailableSlots;
using Domain.Bookings;
using Domain.EventTypes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.AvailableSlots.Get;

public class GetAvailableSlotsQueryHandler(
        IApplicationDbContext _context,
        ISlotCalculator _slotCalculator) : IQueryHandler<GetAvailableSlotsQuery, List<TimeSlotDto>>
{


    public async Task<Result<List<TimeSlotDto>>> Handle(
        GetAvailableSlotsQuery request,
        CancellationToken cancellationToken)
    {
        EventType? eventType = await _context.EventTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EventTypeId, cancellationToken);

        if (eventType == null)
        {
            return Result.Failure<List<TimeSlotDto>>("Event type not found");
        }

        if (!eventType.IsActive)
        {
            return Result.Failure<List<TimeSlotDto>>(EventTypeErrors.Inactive);
        }

        Result<List<TimeSlot>> slotsResult = await _slotCalculator.CalculateAvailableSlots(
            eventType.UserId,
            request.Date,
            eventType.DurationMinutes,
            eventType.BufferMinutes,
            cancellationToken);

        // If calculation failed (DB error, SQLite translation, etc.)
        if (slotsResult.IsFailure)
        {
            return Result.Failure<List<TimeSlotDto>>(BookingErrors.SlotCalculatorFailed);
        }

        List<TimeSlot> slots = slotsResult.Value;

        var slotDtos = slots.Select(s => new TimeSlotDto
        {
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            IsAvailable = s.IsAvailable
        }).ToList();

        return Result<List<TimeSlotDto>>.Success(slotDtos);
    }
}
