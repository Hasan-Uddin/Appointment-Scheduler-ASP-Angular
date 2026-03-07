using Application.Abstractions.Messaging;
using Application.Features.Bookings.Get;
using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Bookings;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, GetBookingQueryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetBookingQuery(id);

            Result<GetBookingQueryResponse> result = await handler.Handle(query, cancellationToken);

            return result.IsSuccess? Results.Ok(result.Value): CustomResults.Problem(result);
        })
        .WithTags(Tags.Bookings)
        .RequireAuthorization();
    }
}
