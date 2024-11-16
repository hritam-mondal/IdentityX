using IdentityX.Core.Dtos;
using IdentityX.Core.Interfaces;

namespace IdentityX.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var authGroup = routes.MapGroup("/api/auth").WithTags("Authentication");

        authGroup.MapPost("/login", async (UserLoginDto request, IAuthService authService) =>
        {
            var response = await authService.LoginAsync(request);
            return Results.Ok(response);
        }).WithName("Login");


        authGroup.MapPost("/register", async (RegisterDto request, IAuthService authService) =>
        {
            var response = await authService.RegisterAsync(request);
            return Results.Ok(response);
        }).WithName("Register");
    }
}