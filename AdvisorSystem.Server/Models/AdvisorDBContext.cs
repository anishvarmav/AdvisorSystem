using Microsoft.EntityFrameworkCore;

public class AdvisorDbContext : DbContext
{
    public DbSet<Advisor> Advisors { get; set; }

    public AdvisorDbContext(DbContextOptions<AdvisorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advisor>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Advisor>().HasIndex(p => p.SIN).IsUnique();
    }
}