
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Bookings.Get;

public sealed class GetBookingQueryHandler(IApplicationDbContext context) : IQueryHandler<GetBookingQuery, GetBookingQueryResponse>
{
    public async Task<Result<GetBookingQueryResponse>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        GetBookingQueryResponse? booking = await context.Bookings
            .AsNoTracking()
            .Where(b => b.Id == query.BookingId)
            .Select(b => new GetBookingQueryResponse
            {
                Id = b.Id,
                EventTypeId = b.EventTypeId,
                EventTypeName = b.EventType.Name,
                GuestName = b.GuestName,
                GuestEmail = b.GuestEmail,
                HostName = b.User.Name,
                HostEmail = b.User.Email,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Duration = (int)(b.EndTime - b.StartTime).TotalMinutes,
                Status = b.Status.ToString(),
                Notes = b.Notes,
                GoogleEventId = b.GoogleEventId,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null)
        {
            return Result.Failure<GetBookingQueryResponse>(BookingErrors.NotFound(query.BookingId));
        }

        return Result.Success(booking);
    }
}
