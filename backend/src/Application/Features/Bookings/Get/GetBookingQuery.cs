
using Application.Abstractions.Messaging;

namespace Application.Features.Bookings.Get;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<GetBookingQueryResponse>;
