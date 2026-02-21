using Application.Abstractions.Messaging;
using Application.Features.AvailableSlots.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AvailableSlots;

public sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/available-slots", async (
            Request request,
            ICommandHandler<CreateAvailabilityCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateAvailabilityCommand(
                request.UserId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.AvailableSlots)
        .RequireAuthorization();
    }
}
