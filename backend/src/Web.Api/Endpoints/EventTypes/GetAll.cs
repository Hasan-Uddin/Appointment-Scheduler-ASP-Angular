using Application.Abstractions.Messaging;
using Application.Features.EventTypes.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.EventTypes;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/event-types", async (
            IQueryHandler<GetAllEventTypesQuery, List<EventTypeResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllEventTypesQuery();

            Result<List<EventTypeResponse>> result =
                await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.EventTypes)
        .RequireAuthorization();
    }
}
