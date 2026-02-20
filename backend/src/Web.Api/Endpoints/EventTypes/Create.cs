using Application.Abstractions.Messaging;
using Application.Features.EventTypes.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.EventTypes;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int BufferMinutes { get; set; }
        public string? Color { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("event-types", async (
            Request request,
            ICommandHandler<CreateEventTypeCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            CreateEventTypeCommand command = new()
            {
                UserId = request.UserId,
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                BufferMinutes = request.BufferMinutes,
                Color = request.Color
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.EventTypes)
        .RequireAuthorization();
    }
}
