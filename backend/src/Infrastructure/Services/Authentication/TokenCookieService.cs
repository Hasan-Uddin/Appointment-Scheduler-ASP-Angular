using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Authentication;

public sealed class TokenCookieService : ITokenCookieService
{
    private readonly IConfiguration _configuration;

    public TokenCookieService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SetAccessToken(HttpContext context, string token)
    {
        int expiryMinutes = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

        CookieOptions options = BuildCookieOptions(expiryMinutes);

        context.Response.Cookies.Append("access_token", token, options);
    }

    public void RefreshAccessToken(HttpContext context, string token)
    {
        SetAccessToken(context, token);
    }

    public void ClearAccessToken(HttpContext context)
    {
        context.Response.Cookies.Delete("access_token");
    }

    private CookieOptions BuildCookieOptions(int expiryMinutes)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
        };
    }
}
