using Microsoft.EntityFrameworkCore;
using IdentityX.Core.Entities;

namespace IdentityX.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );
        
        // Configure composite primary key for UserRole
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // Configure relationships
        modelBuilder.Entity<UserRole>()
            .HasOne<User>() // Explicitly reference the User entity
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        modelBuilder.Entity<UserRole>()
            .HasOne<Role>() // Explicitly reference the Role entity
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }
}