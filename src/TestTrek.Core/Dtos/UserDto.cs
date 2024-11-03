using TestTrek.Core.Entities;

namespace TestTrek.Core.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; } = [];
}