using IdentityX.Core.Dtos;

namespace IdentityX.Core.Interfaces;

public interface IUserService
{
    Task<List<UserDto?>> GetAllUsersAsync();
}