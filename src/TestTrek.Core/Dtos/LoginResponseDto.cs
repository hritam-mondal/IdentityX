namespace TestTrek.Core.Dtos;

public class LoginResponseDto
{
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string> Errors { get; set; } = new();
    public UserInfoDto User { get; set; } = new();
}