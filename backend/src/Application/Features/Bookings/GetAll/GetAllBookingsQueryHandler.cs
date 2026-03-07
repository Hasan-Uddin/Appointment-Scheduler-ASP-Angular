using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Bookings.GetAll;

public sealed class GetAllBookingsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllBookingsQuery, List<GetAllBookingsQueryResponse>>
{
    public async Task<Result<List<GetAllBookingsQueryResponse>>> Handle(GetAllBookingsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Booking> bookingsQuery = context.Bookings
            .AsNoTracking()
            .Include(b => b.EventType)
            .Include(b => b.User);

        // Apply filters
        if (query.UserId.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.UserId == query.UserId.Value);
        }

        if (!string.IsNullOrEmpty(query.Status) &&
            Enum.TryParse<BookingStatus>(query.Status, out BookingStatus status))
        {
            bookingsQuery = bookingsQuery.Where(b => b.Status == status);
        }

        if (query.StartDate.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.StartTime >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.StartTime <= query.EndDate.Value);
        }

        List<GetAllBookingsQueryResponse> bookings = await bookingsQuery
            .OrderByDescending(b => b.StartTime)
            .Select(b => new GetAllBookingsQueryResponse
            {
                Id = b.Id,
                EventTypeName = b.EventType.Name,
                GuestName = b.GuestName,
                GuestEmail = b.GuestEmail,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(bookings);
    }
}
