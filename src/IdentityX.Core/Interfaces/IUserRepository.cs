using IdentityX.Core.Entities;

namespace IdentityX.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameOrEmailAsync(string username, string email);
    Task RegisterUserAsync(User? user);
    Task<List<User>> GetAllUsersAsync();
    Task AddUserRoleAsync(UserRole userRole);
}