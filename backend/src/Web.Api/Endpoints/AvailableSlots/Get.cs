using Application.Abstractions.Messaging;
using Application.Features.AvailableSlots;
using Application.Features.AvailableSlots.Get;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AvailableSlots;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/available-slots/{eventTypeId:guid}/{date}", async (
            Guid eventTypeId,
            DateTime date,
            IQueryHandler<GetAvailableSlotsQuery, List<TimeSlotDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAvailableSlotsQuery(eventTypeId, date);

            Result<List<TimeSlotDto>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.AvailableSlots)
        .WithSummary("Get available slots for an event type on a specific date")
        .AllowAnonymous(); // Guests need to see slots without logging in
    }
}
