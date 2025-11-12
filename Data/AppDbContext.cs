using Microsoft.EntityFrameworkCore;
using TicketSys.Models;

namespace TicketSys.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                FullName = "Admin User",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                Email = "customer@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
                Role = "Customer",
                FullName = "John Doe",
                CreatedAt = DateTime.UtcNow
            }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id = 1,
                Title = "Login issue",
                Description = "Cannot login to my account",
                Status = "Open",
                Priority = "High",
                UserId = 2,
                CreatedAt = DateTime.UtcNow
            },
            new Ticket
            {
                Id = 2,
                Title = "Feature request",
                Description = "Please add dark mode",
                Status = "InProgress",
                Priority = "Low",
                UserId = 2,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}

