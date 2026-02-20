using Application.Abstractions.Messaging;

namespace Application.Features.Bookings.GetAll;

public sealed record GetAllBookingsQuery(
    Guid? UserId = null,
    string? Status = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IQuery<List<GetAllBookingsQueryResponse>>;
