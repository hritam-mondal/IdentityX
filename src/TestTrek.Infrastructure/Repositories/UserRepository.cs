using Microsoft.EntityFrameworkCore;
using TestTrek.Core.Entities;
using TestTrek.Core.Interfaces;
using TestTrek.Infrastructure.Data;

namespace TestTrek.Infrastructure.Repositories;

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