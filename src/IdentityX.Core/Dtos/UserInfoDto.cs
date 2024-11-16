using IdentityX.Core.Entities;

namespace IdentityX.Core.Dtos;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public List<UserRole> Role { get; set; } = [];
}