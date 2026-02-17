using Application.Abstractions.Messaging;
using Application.Features.Auth.Login;
using SharedKernel;

namespace Web.Api.Endpoints.Auth;

public class GoogleCallback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/google/callback",
            async (
                string? code,
                IConfiguration configuration,
                ICommandHandler<GoogleLoginCommand, GoogleLoginCommandResponse> handler,
                CancellationToken ct) =>
            {
                var command = new GoogleLoginCommand(code ?? string.Empty);
                Result<GoogleLoginCommandResponse> result = await handler.Handle(command, ct);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }
                string? frontendBase = configuration["Frontend:BaseUrl"];
                string redirectUrl = $"{frontendBase}/auth?token={result.Value.Jwt}";
                return Results.Redirect(redirectUrl);
            }
        ).WithTags(Tags.Auth);
    }
}
