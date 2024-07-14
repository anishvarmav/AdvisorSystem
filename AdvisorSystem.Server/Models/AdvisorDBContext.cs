using Microsoft.EntityFrameworkCore;

public class AdvisorDbContext : DbContext
{
    public DbSet<Advisor> Advisors { get; set; }

    public AdvisorDbContext(DbContextOptions<AdvisorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set the Id property to be generated and auto incremented.
        modelBuilder.Entity<Advisor>().Property(p => p.Id).ValueGeneratedOnAdd();
        // Create a unique index on SIN to prevent duplicates. Not seems to be working.
        modelBuilder.Entity<Advisor>().HasIndex(p => p.SIN).IsUnique();
    }
}