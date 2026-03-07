using Application.Abstractions.Messaging;
using Application.Features.EventTypes.GetByEmailAndSlug;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.EventTypes;

internal sealed class GetByEmailAndSlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/public/event-types/{email}/{slug}", async (
            string email,
            string slug,
            IQueryHandler<GetEventTypeByEmailAndSlugQuery, EventTypeResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetEventTypeByEmailAndSlugQuery(email+"@gmail.com", slug);

            Result<EventTypeResponse> result =
                await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.EventTypes);
    }
}
