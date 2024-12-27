using IdentityX.Core.Entities;

namespace IdentityX.Core.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByNameAsync(string name);
    Task AddRoleAsync(Role? role);
}