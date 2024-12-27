using IdentityX.Core.Dtos;

namespace IdentityX.Core.Interfaces;

public interface IRoleService
{
    Task<RoleDto?> CreateRoleAsync(RoleDto roleDto);
}