namespace IdentityX.Core.Dtos;

public class UserLoginDto
{
    public string Identifier { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}