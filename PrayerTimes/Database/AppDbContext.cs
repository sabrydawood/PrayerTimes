using PrayerTimes.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<SettingsEntity> Settings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entity properties here if needed
        base.OnModelCreating(modelBuilder);
    }
}
