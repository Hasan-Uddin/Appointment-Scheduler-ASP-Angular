using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions.Authentication;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Authentication;

public class GoogleAuthService(
    HttpClient _httpClient,
    IConfiguration _configuration
    ) : IGoogleAuthService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<GoogleUserInfo> ExchangeCodeAsync(string code)
    {
        TokenResponse tokenResponse = await RequestTokenAsync(code);

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt = handler.ReadJwtToken(tokenResponse.IdToken);

        string email = jwt.Claims.First(x => x.Type == "email").Value;
        string name = jwt.Claims.First(x => x.Type == "name").Value;
        string sub = jwt.Claims.First(x => x.Type == "sub").Value;
        string? pictureUrl = jwt.Claims.FirstOrDefault(x => x.Type == "picture")?.Value;

        return new GoogleUserInfo
        {
            Email = email,
            Name = name,
            GoogleId = sub,
            PictureUrl = pictureUrl ?? ""
        };
    }

    private async Task<TokenResponse> RequestTokenAsync(string code)
    {
        string tokenEndpoint = "https://oauth2.googleapis.com/token";

        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _configuration["Google:ClientId"]! },
            { "client_secret", _configuration["Google:ClientSecret"]! },
            { "redirect_uri", _configuration["Google:RedirectUri"]! },
            { "grant_type", "authorization_code" }
        });

        HttpResponseMessage response = await _httpClient.PostAsync(tokenEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Google token exchange failed: {error}");
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);

        return JsonSerializer.Deserialize<TokenResponse>(json, _jsonOptions)!;
    }

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = default!;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = default!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpireIn { get; set; }

        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}
