using IdentityX.Core.Dtos;

namespace IdentityX.Core.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(UserLoginDto userLoginDto);
}