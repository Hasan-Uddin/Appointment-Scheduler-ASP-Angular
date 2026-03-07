using Application.Abstractions.Messaging;
using Application.Features.Available.Get;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Availability;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/availabilities", async (
            IQueryHandler<GetAvailabilitiesQuery, List<AvailabilityResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAvailabilitiesQuery();

            Result<List<AvailabilityResponse>> result =
                await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Availability)
        .RequireAuthorization();
    }
}
