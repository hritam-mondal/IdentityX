using IdentityX.Core.Dtos;
using IdentityX.Core.Entities;
using IdentityX.Core.Interfaces;

namespace IdentityX.Services.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<RoleDto?> CreateRoleAsync(RoleDto roleDto)
    {
        // Check if the role already exists
        var existingRole = await roleRepository.GetRoleByNameAsync(roleDto.Name);
        if (existingRole != null)
        {
            return null; // Role already exists
        }

        // Create the new role
        var newRole = new Role { Name = roleDto.Name };
        await roleRepository.AddRoleAsync(newRole);

        return new RoleDto { Name = newRole.Name };
    }
}