using Application.Abstractions.Data;
using Application.Abstractions.Interfaces;
using Domain.Availabilities;
using Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.SlotCalculator;

public class SlotCalculatorService(IApplicationDbContext context) : ISlotCalculator
{

    public async Task<List<TimeSlot>> CalculateAvailableSlots(
        Guid userId,
        DateTime date,
        int durationMinutes,
        int bufferMinutes,
        CancellationToken cancellationToken)
    {
        DayOfWeek dayOfWeek = date.DayOfWeek;

        // Get user's availability for the day
        List<Availability> availabilities = await context.Availabilities
            .Where(a => a.UserId == userId &&
                       a.DayOfWeek == dayOfWeek &&
                       a.IsActive)
            .ToListAsync(cancellationToken);

        if (!availabilities.Any())
        {
            return new List<TimeSlot>();
        }

        // Get existing bookings for the day
        DateTime startOfDay = date.Date;
        DateTime endOfDay = startOfDay.AddDays(1);

        List<Booking> existingBookings = await context.Bookings
            .Where(b => b.UserId == userId &&
                       b.Status == BookingStatus.Confirmed &&
                       b.StartTime >= startOfDay &&
                       b.StartTime < endOfDay)
            .ToListAsync(cancellationToken);

        var slots = new List<TimeSlot>();

        foreach (Availability availability in availabilities)
        {
            DateTime currentTime = date.Date.Add(availability.StartTime.ToTimeSpan());
            DateTime endTime = date.Date.Add(availability.EndTime.ToTimeSpan());

            while (currentTime.Add(TimeSpan.FromMinutes(durationMinutes)) <= endTime)
            {
                DateTime slotEnd = currentTime.Add(TimeSpan.FromMinutes(durationMinutes));

                // Check if slot conflicts with existing bookings
                bool hasConflict = existingBookings.Any(b =>
                    currentTime >= b.StartTime && currentTime < b.EndTime ||
                    slotEnd > b.StartTime && slotEnd <= b.EndTime);

                // Only add future slots
                if (currentTime > DateTime.UtcNow && !hasConflict)
                {
                    slots.Add(new TimeSlot
                    {
                        StartTime = currentTime,
                        EndTime = slotEnd,
                        IsAvailable = true
                    });
                }

                currentTime = currentTime.Add(TimeSpan.FromMinutes(durationMinutes + bufferMinutes));
            }
        }

        return slots.OrderBy(s => s.StartTime).ToList();
    }

    public async Task<bool> IsSlotAvailable(
        Guid userId,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken)
    {
        DayOfWeek dayOfWeek = startTime.DayOfWeek;
        var timeOnly = TimeOnly.FromDateTime(startTime);

        // Check if time falls within availability
        bool hasAvailability = await context.Availabilities
            .AnyAsync(a =>
                a.UserId == userId &&
                a.DayOfWeek == dayOfWeek &&
                a.IsActive &&
                timeOnly >= a.StartTime &&
                timeOnly < a.EndTime,
                cancellationToken);

        if (!hasAvailability)
        {
            return false;
        }

        // Check for conflicts
        bool hasConflict = await context.Bookings
            .AnyAsync(b =>
                b.UserId == userId &&
                b.Status == BookingStatus.Confirmed &&
                (startTime >= b.StartTime && startTime < b.EndTime ||
                 endTime > b.StartTime && endTime <= b.EndTime),
                cancellationToken);

        return !hasConflict;
    }
}

