using Mapster;
using TestTrek.Core.Dtos;
using TestTrek.Core.Interfaces;

namespace TestTrek.Services.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<UserDto?>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        return users.Adapt<List<UserDto?>>();
    }
}