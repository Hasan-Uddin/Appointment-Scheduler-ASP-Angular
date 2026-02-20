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
                    HttpContext httpContext,
                    CancellationToken ct) =>
                {
                    var command = new GoogleLoginCommand(code ?? string.Empty);
                    Result<GoogleLoginCommandResponse> result = await handler.Handle(command, ct);
            
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    string jwt = result.Value.Jwt;
                    string? frontendBase = configuration["Frontend:BaseUrl"];
            
                    httpContext.Response.Cookies.Append(
                        "access_token",
                        jwt,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // MUST be true in production
                            SameSite = SameSiteMode.None, // Required for cross-site OAuth
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });
                    return Results.Redirect(frontendBase!);
                }
            )
            .WithTags(Tags.Auth);
    }
}
