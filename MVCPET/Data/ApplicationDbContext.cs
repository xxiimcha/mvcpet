using Microsoft.EntityFrameworkCore;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetVaccination> Vaccinations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AdoptionRequest> AdoptionRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Precomputed hash of "Admin@123"
        string staticHashedPassword = "$2a$11$zLxA8hdOfJexQl9DaiAvAO4T4dKpH5wsNBNI9dKH4zE0ZWK4zKz3O";

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1, // Ensure this ID is unique
            Name = "Admin User",
            Email = "admin@example.com",
            Password = staticHashedPassword, // Store hashed password
            Address = "Admin HQ",
            Role = "Admin",
            CreatedAt = new DateTime(2024, 1, 1) // Static DateTime to prevent errors
        });
    }
}
