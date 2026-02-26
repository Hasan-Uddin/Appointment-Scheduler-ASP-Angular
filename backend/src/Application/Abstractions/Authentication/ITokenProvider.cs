using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
    TokenResult Create(User user);
}

public sealed record TokenResult(
    string AccessToken,
    DateTime ExpiresAtUtc);
