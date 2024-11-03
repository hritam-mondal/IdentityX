using TestTrek.Core.Interfaces;
using TestTrek.Services.Services;

namespace TestTrek.API.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder routes)
    {
        var userGroup = routes.MapGroup("/api/users").WithTags("User Management");
        
        userGroup.MapGet("/", async (IUserService userService) =>
        {
            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);
        }).RequireAuthorization().WithName("GGetAllUsers");
    }
}