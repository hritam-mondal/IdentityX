using TestTrek.Core.Dtos;

namespace TestTrek.Core.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(UserLoginDto userLoginDto);
}