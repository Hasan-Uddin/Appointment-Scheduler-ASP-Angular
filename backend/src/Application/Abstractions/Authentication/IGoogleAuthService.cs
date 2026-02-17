
namespace Application.Abstractions.Authentication;

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> ExchangeCodeAsync(string code);
}

public class GoogleUserInfo
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public string GoogleId { get; set; }
}
