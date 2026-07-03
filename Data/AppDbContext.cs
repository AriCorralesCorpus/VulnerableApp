using Microsoft.EntityFrameworkCore;
using VulnerableApp.Models;
namespace VulnerableApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "$2a$11$EjemploHashAdmin",
                Email = "admin@test.com",
                Balance = 1000m,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                Username = "user1",
                PasswordHash = "$2a$11$EjemploHashUser1",
                Email = "user@test.com",
                Balance = 500m,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 3,
                Username = "user2",
                PasswordHash = "$2a$11$EjemploHashUser2",
                Email = "user2@test.com",
                Balance = 750m,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }

    }
}