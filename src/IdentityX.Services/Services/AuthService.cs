using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using IdentityX.Core.Dtos;
using IdentityX.Core.Entities;
using IdentityX.Core.Interfaces;

namespace IdentityX.Services.Services;

public class AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
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
        };

        await userRepository.RegisterUserAsync(user);
        // Retrieve the role
        var role = await roleRepository.GetRoleByNameAsync(registerDto.Role).ConfigureAwait(false);
        if (role == null)
        {
            logger.LogError("Role not found: {Role}", registerDto.Role);
            return GenerateErrorResponse("role", "Invalid role.");
        }

        // Create UserRole using the persisted UserId
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };
        Console.WriteLine(userRole);

        // Associate the UserRole with the user
        user.UserRoles = [userRole];

        // Save the UserRole to the database
        await userRepository.AddUserRoleAsync(userRole).ConfigureAwait(false);

        logger.LogInformation("User registered successfully with roles: {Username}", user.Username);


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
        logger.LogInformation("Login attempt for user: {UsernameOrEmail}", userLoginDto.Identifier);
    
        // Determine if input is an email or username
        var isEmail = userLoginDto.Identifier.Contains('@');
        var user = isEmail
            ? await userRepository.GetUserByEmailAsync(userLoginDto.Identifier ) // Fetch user by email
            : await userRepository.GetUserByUsernameAsync(userLoginDto.Identifier ); // Fetch user by username
    
        if (user == null)
        {
            logger.LogWarning("Login failed: User with Username or Email: {UsernameOrEmail} not found.", userLoginDto.Identifier );
            return new LoginResponseDto
            {
                Status = "error",
                Message = "Login failed.",
                Errors = new Dictionary<string, string>
                {
                    { "credentials", "Invalid username/email or password." }
                }
            };
        }
    
        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
        {
            logger.LogWarning("Login failed: Invalid password for Username or Email: {UsernameOrEmail}", userLoginDto.Identifier);
            return new LoginResponseDto
            {
                Status = "error",
                Message = "Login failed.",
                Errors = new Dictionary<string, string>
                {
                    { "credentials", "Invalid username/email or password." }
                }
            };
        }
    
        // Generate token
        var token = GenerateJwtToken(user);
        logger.LogInformation("Login successful for user: {UsernameOrEmail}", userLoginDto.Identifier);
    
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
    
    private static RegisterResponseDto GenerateErrorResponse(string field, string errorMessage)
    {
        return new RegisterResponseDto
        {
            Status = "error",
            Message = "Registration failed.",
            Errors = new Dictionary<string, string>
            {
                { field, errorMessage }
            }
        };
    }
}
