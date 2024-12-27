using IdentityX.Core.Entities;
using IdentityX.Core.Interfaces;
using IdentityX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityX.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await context.Roles.FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task AddRoleAsync(Role? role)
    {
        if (role != null) context.Roles.Add(role);
        await context.SaveChangesAsync();
    }
}