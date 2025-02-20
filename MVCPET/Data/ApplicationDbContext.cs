using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetVaccination> Vaccinations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
}
