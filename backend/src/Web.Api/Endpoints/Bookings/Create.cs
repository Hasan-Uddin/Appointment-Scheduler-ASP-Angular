using Application.Abstractions.Messaging;
using Application.Features.Bookings.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Bookings;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid EventTypeId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public string? Notes { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/bookings/create", async (
            Request request,
            ICommandHandler<CreateBookingCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateBookingCommand(
                request.EventTypeId,
                request.GuestName,
                request.GuestEmail,
                request.StartTime,
                request.Notes
             );

            try
            {
                Result<Guid> result = await handler.Handle(command, cancellationToken);
                return result.Match(Results.Ok, CustomResults.Problem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        })
        .WithTags(Tags.Bookings)
        .AllowAnonymous(); // Public booking link scenario
    }
}
