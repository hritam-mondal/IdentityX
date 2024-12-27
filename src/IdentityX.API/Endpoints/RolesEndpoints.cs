using IdentityX.Core.Dtos;
using IdentityX.Core.Interfaces;

namespace IdentityX.API.Endpoints;

public static class RolesEndpoints
{
    public static void MapRolesEndpoints(this IEndpointRouteBuilder routes)
    {
        var roleGroup = routes.MapGroup("/api/roles").WithTags("Role Management");

        roleGroup.MapPost("/", async (RoleDto roleDto, IRoleService roleService) =>
        {
            // Create the new role using the service
            var createdRole = await roleService.CreateRoleAsync(roleDto);

            return createdRole == null ? Results.BadRequest(new { message = "Failed to create role." }) : Results.Ok(createdRole);
        }).RequireAuthorization().WithName("CreateRole");
    }
}