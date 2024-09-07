using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PrayerTimes.Database
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connString = "Data Source=Database.db"; // Ensure this matches your configuration
            optionsBuilder.UseSqlite(connString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
