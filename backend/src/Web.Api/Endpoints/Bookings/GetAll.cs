using Application.Abstractions.Messaging;
using Application.Features.Bookings.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Bookings;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/bookings", async (
            Guid? UserId,
            string? Status,
            DateTime? StartDate,
            DateTime? EndDate,
            IQueryHandler <GetAllBookingsQuery, List<GetAllBookingsQueryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllBookingsQuery(
                UserId,
                Status,
                StartDate,
                EndDate);

            Result <List<GetAllBookingsQueryResponse>> result =
                await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Bookings)
        .RequireAuthorization();
    }
}
