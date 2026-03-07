
using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Availabilities;
using Domain.Bookings;
using Domain.EventTypes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.AvailableSlots.GetMontlyList;

internal sealed class GetAvailableDaysQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetAvailableDaysQuery, List<AvailableDayResponse>>
{
    public async Task<Result<List<AvailableDayResponse>>> Handle(
        GetAvailableDaysQuery query,
        CancellationToken cancellationToken)
    {
        if (query.Month < 1 || query.Month > 12)
        {
            return Result.Failure<List<AvailableDayResponse>>("Invalid month.");
        }

        if (query.Year < 1)
        {
            return Result.Failure<List<AvailableDayResponse>>("Invalid year.");
        }

        EventType? eventType = await context.EventTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == query.EventTypeId && e.IsActive,
                cancellationToken);

        if (eventType is null)
        {
            return Result.Failure<List<AvailableDayResponse>>("Event type not found.");
        }

        Guid userId = eventType.UserId;

        var monthStart = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime monthEnd = monthStart.AddMonths(1);

        // Load weekly availabilities once
        List<Availability> availabilities = await context.Availabilities
            .AsNoTracking()
            .Where(a => a.UserId == userId && a.IsActive)
            .ToListAsync(cancellationToken);

        // Load bookings overlapping month once
        List<Booking> bookings = await context.Bookings
            .AsNoTracking()
            .Where(b =>
                b.UserId == userId &&
                b.Status == BookingStatus.Confirmed &&
                b.StartTime < monthEnd &&
                b.EndTime > monthStart)
            .ToListAsync(cancellationToken);

        var result = new List<AvailableDayResponse>();

        int daysInMonth = DateTime.DaysInMonth(query.Year, query.Month);
        DateTime utcNow = DateTime.UtcNow;

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(
                query.Year,
                query.Month,
                day,
                0, 0, 0,
                DateTimeKind.Utc);

            DayOfWeek dayOfWeek = date.DayOfWeek;

            var dayAvailabilities = availabilities
                .Where(a => a.DayOfWeek == dayOfWeek)
                .ToList();

            if (!dayAvailabilities.Any())
            {
                result.Add(new AvailableDayResponse
                {
                    Date = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    HasSlots = false,
                    SlotsCount = 0
                });

                continue;
            }

            int slotsCount = 0;

            foreach (Availability availability in dayAvailabilities)
            {
                DateTime availabilityStart = date.Add(
                    availability.StartTime.ToTimeSpan());

                DateTime availabilityEnd = date.Add(
                    availability.EndTime.ToTimeSpan());

                DateTime currentTime = availabilityStart;

                while (currentTime.AddMinutes(eventType.DurationMinutes)
                       <= availabilityEnd)
                {
                    DateTime slotEnd =
                        currentTime.AddMinutes(eventType.DurationMinutes);

                    bool hasConflict = bookings.Any(b =>
                    {
                        DateTime bufferedStart =
                            b.StartTime.AddMinutes(-eventType.BufferMinutes);

                        DateTime bufferedEnd =
                            b.EndTime.AddMinutes(eventType.BufferMinutes);

                        return currentTime < bufferedEnd &&
                               slotEnd > bufferedStart;
                    });

                    if (currentTime > utcNow && !hasConflict)
                    {
                        slotsCount++;
                    }

                    currentTime = currentTime.AddMinutes(
                        eventType.DurationMinutes + eventType.BufferMinutes);
                }
            }

            result.Add(new AvailableDayResponse
            {
                Date = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                HasSlots = slotsCount > 0,
                SlotsCount = slotsCount
            });
        }

        return Result.Success(result);
    }
}
