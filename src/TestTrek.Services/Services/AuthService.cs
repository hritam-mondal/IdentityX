using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TestTrek.Core.Dtos;
using TestTrek.Core.Entities;
using TestTrek.Core.Interfaces;

namespace TestTrek.Services.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        logger.LogInformation("Register attempt for user: {Username}, Email: {Email}", registerDto.Username, registerDto.Email);

        // Check if user already exists
        var existingUser = await userRepository.GetUserByUsernameOrEmailAsync(registerDto.Username, registerDto.Email);
        if (existingUser != null)
        {
            logger.LogWarning("Registration failed: User with Username: {Username} or Email: {Email} already exists.", registerDto.Username, registerDto.Email);
            return new RegisterResponseDto
            {
                Status = "error",
                Message = "Registration failed.",
                Errors = new Dictionary<string, string>
                {
                    { "username", "User already exists." }
                }
            };
        }

        // Hash the password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        logger.LogDebug("Password hashed for user: {Username}", registerDto.Username);

        // Create new user
        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username,
            PasswordHash = hashedPassword,
            UserRoles = [] // Initialize as empty
        };

        await userRepository.RegisterUserAsync(user);
        logger.LogInformation("User registered successfully: {Username}", user.Username);

        // Generate token
        var token = GenerateJwtToken(user);
        logger.LogDebug("JWT token generated for user: {Username}", user.Username);

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
        logger.LogInformation("Login attempt for user: {Username}", userLoginDto.Username);

        // Fetch the user
        var user = await userRepository.GetUserByUsernameAsync(userLoginDto.Username);
        if (user == null)
        {
            logger.LogWarning("Login failed: User with Username: {Username} not found.", userLoginDto.Username);
            return new LoginResponseDto
            {
                Status = "error",
                Message = "Login failed.",
                Errors = new Dictionary<string, string>
                {
                    { "credentials", "Invalid username or password." }
                }
            };
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
        {
            logger.LogWarning("Login failed: Invalid password for Username: {Username}", userLoginDto.Username);
            return new LoginResponseDto
            {
                Status = "error",
                Message = "Login failed.",
                Errors = new Dictionary<string, string>
                {
                    { "credentials", "Invalid username or password." }
                }
            };
        }

        // Generate token
        var token = GenerateJwtToken(user);
        logger.LogInformation("Login successful for user: {Username}", userLoginDto.Username);

        return new LoginResponseDto
        {
            Status = "success",
            Message = "Login successful.",
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

    private string GenerateJwtToken(User? user)
    {
        logger.LogDebug("Generating JWT token for user: {Username}", user?.Username);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user!.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        logger.LogDebug("JWT token created successfully for user: {Username}", user?.Username);

        return tokenHandler.WriteToken(token);
    }
}
