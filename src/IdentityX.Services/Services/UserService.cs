using Mapster;
using Microsoft.Extensions.Logging;
using IdentityX.Core.Dtos;
using IdentityX.Core.Interfaces;

namespace IdentityX.Services.Services;

public class UserService(IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    public async Task<List<UserDto?>> GetAllUsersAsync()
    {
        logger.LogInformation("Fetching all users from the database.");
        var users = await userRepository.GetAllUsersAsync();
        logger.LogDebug("Fetched {Count} users from the database.", users.Count);
        
        var userDtos = users.Adapt<List<UserDto?>>();
        logger.LogInformation("Successfully mapped users to UserDto.");
            
        return userDtos;
    }
}