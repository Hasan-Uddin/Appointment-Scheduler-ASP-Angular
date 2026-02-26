using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Authentication;

public class JwtRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITokenCookieService _cookieService;

    public JwtRefreshMiddleware(
        RequestDelegate next,
        ITokenProvider tokenService,
        ITokenCookieService cookieService)
    {
        _next = next;
        _tokenProvider = tokenService;
        _cookieService = cookieService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies["access_token"];

        if (!string.IsNullOrEmpty(token))
        {
            if (_tokenProvider.ShouldRefresh(token))
            {
                var newToken = _tokenProvider.Refresh(token);
                _cookieService.RefreshAccessToken(context, newToken);
            }
        }

        await _next(context);
    }
}
