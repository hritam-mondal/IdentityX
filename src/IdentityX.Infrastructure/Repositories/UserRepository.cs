using Microsoft.EntityFrameworkCore;
using IdentityX.Core.Entities;
using IdentityX.Core.Interfaces;
using IdentityX.Infrastructure.Data;

namespace IdentityX.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByUsernameOrEmailAsync(string username, string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => (u.Username == username || u.Email == email));
    }

    public async Task RegisterUserAsync(User? user)
    {
        if (user != null) context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<List<User?>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }
}