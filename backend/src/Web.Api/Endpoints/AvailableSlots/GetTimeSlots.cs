using System.Globalization;
using Application.Abstractions.Messaging;
using Application.Features.Availability.AvailableSlots.GetAvailableTImeSlot;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AvailableSlots;

internal sealed class GetTimeSlots : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/public/event-types/{eventTypeId:guid}/slots",
            async (
                Guid eventTypeId,
                string date,
                IQueryHandler<GetTimeSlotsQuery, List<TimeSlotResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                if (!DateTime.TryParseExact(
                    date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out DateTime parsedDate))
                {
                    return Results.BadRequest("Invalid date format. Use YYYY-MM-DD.");
                }

                var query = new GetTimeSlotsQuery(
                    eventTypeId,
                    parsedDate);

                Result<List<TimeSlotResponse>> result =
                    await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
        .WithTags(Tags.EventTypes);
    }
}
