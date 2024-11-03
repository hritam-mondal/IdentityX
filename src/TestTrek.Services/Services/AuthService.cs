using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestTrek.Core.Dtos;
using TestTrek.Core.Entities;
using TestTrek.Core.Interfaces;

namespace TestTrek.Services.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
{
    public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await userRepository.GetUserByUsernameOrEmailAsync(registerDto.Username, registerDto.Email);
        if (existingUser != null)
            return new RegisterResponseDto
            {
                Status = "error",
                Message = "Registration failed.",
                Errors = new Dictionary<string, string>
                {
                    { "username", "User already exists." }
                }
            };

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username,
            PasswordHash = hashedPassword,
            UserRoles = []
        };

        await userRepository.RegisterUserAsync(user);
        var token = GenerateJwtToken(user);

        return new RegisterResponseDto
        {
            Status = "success",
            Message = "Registration successful.",
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                Role = user.UserRoles
            }
        };
    }

    public async Task<LoginResponseDto> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await userRepository.GetUserByUsernameAsync(userLoginDto.Username);
        if (user != null && !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            return new LoginResponseDto
            {
                Status = "error",
                Message = "Login failed.",
                Errors = new Dictionary<string, string>
                {
                    { "credentials", "Invalid username or password." }
                }
            };

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Status = "success",
            Message = "Login successful.",
            User = new UserInfoDto
            {
                Id = user!.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                Role = user.UserRoles
            }
        };
    }

    private string GenerateJwtToken(User? user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user!.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}