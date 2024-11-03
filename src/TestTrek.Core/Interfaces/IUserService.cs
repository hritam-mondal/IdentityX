using TestTrek.Core.Dtos;

namespace TestTrek.Core.Interfaces;

public interface IUserService
{
    Task<List<UserDto?>> GetAllUsersAsync();
}