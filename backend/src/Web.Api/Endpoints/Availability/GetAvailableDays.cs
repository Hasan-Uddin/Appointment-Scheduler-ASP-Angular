using Application.Abstractions.Messaging;
using Application.Features.Available.AvailableSlots.GetMontlyList;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Availability;

internal sealed class GetAvailableDays : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/public/availability/{eventTypeId:guid}/available-days",
            async (
                Guid eventTypeId,
                int year,
                int month,
                IQueryHandler<GetAvailableDaysQuery, List<AvailableDayResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAvailableDaysQuery(
                    eventTypeId,
                    year,
                    month);

                Result<List<AvailableDayResponse>> result =
                    await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
        .WithTags(Tags.Availability);
    }
}
