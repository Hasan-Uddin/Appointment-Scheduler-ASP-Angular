using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Authentication;

public interface ITokenCookieService
{
    void SetAccessToken(HttpContext context, string token);
    void RefreshAccessToken(HttpContext context, string token);
    void ClearAccessToken(HttpContext context);
}
