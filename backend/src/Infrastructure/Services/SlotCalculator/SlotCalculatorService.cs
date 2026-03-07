using Application.Abstractions.Data;
using Application.Abstractions.Interfaces;
using Domain.Availabilities;
using Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Services.SlotCalculator;


public class SlotCalculatorService(IApplicationDbContext _context) : ISlotCalculator
{
    public async Task<Result<bool>> IsSlotAvailable(
        Guid userId,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken)
    {
        try
        {
            // Ensure UTC consistency
            startTime = DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
            endTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc);

            DayOfWeek dayOfWeek = startTime.DayOfWeek;
            DateTime date = startTime.Date;

            // Load availability first (avoid EF TimeSpan translation issue)
            List<Availability> availabilities = await _context.Availabilities
                .Where(a => a.UserId == userId &&
                            a.DayOfWeek == dayOfWeek &&
                            a.IsActive)
                .ToListAsync(cancellationToken);

            if (!availabilities.Any())
            {
                return Result.Success(false);
            }

            // Validate slot fits inside availability window
            bool fitsAvailability = availabilities.Any(a =>
            {
                DateTime availabilityStart = date.Add(a.StartTime.ToTimeSpan());
                DateTime availabilityEnd = date.Add(a.EndTime.ToTimeSpan());

                return startTime >= availabilityStart &&
                       endTime <= availabilityEnd;
            });

            if (!fitsAvailability)
            {
                return Result.Success(false);
            }

            // Proper overlap check (handles all cases)
            bool hasConflict = await _context.Bookings
                .AnyAsync(b =>
                    b.UserId == userId &&
                    b.Status == BookingStatus.Confirmed &&
                    startTime < b.EndTime &&
                    endTime > b.StartTime,
                    cancellationToken);

            return Result.Success(!hasConflict);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Slot check failed: {ex.Message}");
        }
    }

    public async Task<Result<List<TimeSlot>>> CalculateAvailableSlots(
        Guid userId,
        DateTime date,
        int durationMinutes,
        int bufferMinutes,
        CancellationToken cancellationToken)
    {
        try
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            DayOfWeek dayOfWeek = date.DayOfWeek;
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1);

            List<Availability> availabilities = await _context.Availabilities
                .Where(a => a.UserId == userId &&
                            a.DayOfWeek == dayOfWeek &&
                            a.IsActive)
                .ToListAsync(cancellationToken);

            if (!availabilities.Any())
            {
                return Result.Failure<List<TimeSlot>>("No availabilities found for this day.");
            }

            // Proper overlap query (handles midnight edge cases)
            List<Booking> existingBookings = await _context.Bookings
                .Where(b => b.UserId == userId &&
                            b.Status == BookingStatus.Confirmed &&
                            b.StartTime < endOfDay &&
                            b.EndTime > startOfDay)
                .ToListAsync(cancellationToken);

            var slots = new List<TimeSlot>();
            DateTime utcNow = DateTime.UtcNow;

            foreach (Availability availability in availabilities)
            {
                DateTime availabilityStart = startOfDay.Add(availability.StartTime.ToTimeSpan());
                DateTime availabilityEnd = startOfDay.Add(availability.EndTime.ToTimeSpan());

                DateTime currentTime = availabilityStart;

                while (currentTime.AddMinutes(durationMinutes) <= availabilityEnd)
                {
                    DateTime slotEnd = currentTime.AddMinutes(durationMinutes);

                    bool hasConflict = existingBookings.Any(b =>
                    {
                        DateTime bufferedStart = b.StartTime.AddMinutes(-bufferMinutes);
                        DateTime bufferedEnd = b.EndTime.AddMinutes(bufferMinutes);

                        return currentTime < bufferedEnd && slotEnd > bufferedStart;
                    });

                    if (currentTime > utcNow && !hasConflict)
                    {
                        slots.Add(new TimeSlot
                        {
                            StartTime = currentTime,
                            EndTime = slotEnd,
                            IsAvailable = true
                        });
                    }

                    currentTime = currentTime.AddMinutes(durationMinutes + bufferMinutes);
                }
            }

            return Result.Success(
                slots.OrderBy(s => s.StartTime).ToList());
        }
        catch (Exception ex)
        {
            return Result.Failure<List<TimeSlot>>(
                $"Failed to calculate slots: {ex.Message}");
        }
    }
}

//public class SlotCalculatorService(IApplicationDbContext context) : ISlotCalculator
//{

//    public async Task<List<TimeSlot>> CalculateAvailableSlots(
//        Guid userId,
//        DateTime date,
//        int durationMinutes,
//        int bufferMinutes,
//        CancellationToken cancellationToken)
//    {
//        DayOfWeek dayOfWeek = date.DayOfWeek;

//        // Get user's availability for the day
//        List<Availability> availabilities = await context.Availabilities
//            .Where(a => a.UserId == userId &&
//                       a.DayOfWeek == dayOfWeek &&
//                       a.IsActive)
//            .ToListAsync(cancellationToken);

//        if (!availabilities.Any())
//        {
//            return new List<TimeSlot>();
//        }

//        // Get existing bookings for the day
//        DateTime startOfDay = date.Date;
//        DateTime endOfDay = startOfDay.AddDays(1);

//        List<Booking> existingBookings = await context.Bookings
//            .Where(b => b.UserId == userId &&
//                       b.Status == BookingStatus.Confirmed &&
//                       b.StartTime >= startOfDay &&
//                       b.StartTime < endOfDay)
//            .ToListAsync(cancellationToken);

//        var slots = new List<TimeSlot>();

//        foreach (Availability availability in availabilities)
//        {
//            DateTime currentTime = date.Date.Add(availability.StartTime.ToTimeSpan());
//            DateTime endTime = date.Date.Add(availability.EndTime.ToTimeSpan());

//            while (currentTime.Add(TimeSpan.FromMinutes(durationMinutes)) <= endTime)
//            {
//                DateTime slotEnd = currentTime.Add(TimeSpan.FromMinutes(durationMinutes));

//                // Check if slot conflicts with existing bookings
//                bool hasConflict = existingBookings.Any(b =>
//                    currentTime >= b.StartTime && currentTime < b.EndTime ||
//                    slotEnd > b.StartTime && slotEnd <= b.EndTime);

//                // Only add future slots
//                if (currentTime > DateTime.UtcNow && !hasConflict)
//                {
//                    slots.Add(new TimeSlot
//                    {
//                        StartTime = currentTime,
//                        EndTime = slotEnd,
//                        IsAvailable = true
//                    });
//                }

//                currentTime = currentTime.Add(TimeSpan.FromMinutes(durationMinutes + bufferMinutes));
//            }
//        }

//        return slots.OrderBy(s => s.StartTime).ToList();
//    }

//    public async Task<bool> IsSlotAvailable(
//        Guid userId,
//        DateTime startTime,
//        DateTime endTime,
//        CancellationToken cancellationToken)
//    {
//        DayOfWeek dayOfWeek = startTime.DayOfWeek;
//        var timeOnly = TimeOnly.FromDateTime(startTime);

//        // Check if time falls within availability
//        bool hasAvailability = await context.Availabilities
//            .AnyAsync(a =>
//                a.UserId == userId &&
//                a.DayOfWeek == dayOfWeek &&
//                a.IsActive &&
//                timeOnly >= a.StartTime &&
//                timeOnly < a.EndTime,
//                cancellationToken);

//        if (!hasAvailability)
//        {
//            return false;
//        }

//        // Check for conflicts
//        bool hasConflict = await context.Bookings
//            .AnyAsync(b =>
//                b.UserId == userId &&
//                b.Status == BookingStatus.Confirmed &&
//                (startTime >= b.StartTime && startTime < b.EndTime ||
//                 endTime > b.StartTime && endTime <= b.EndTime),
//                cancellationToken);

//        return !hasConflict;
//    }
//}

