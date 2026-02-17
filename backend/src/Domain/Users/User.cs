using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string GoogleId { get; set; }
    public string PictureUrl { get; set; }
    public string? RefreshToken { get; set; }
    public string? PasswordHash { get; set; }
    public string? TimeZone { get; set; }
}
