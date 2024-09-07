using Microsoft.EntityFrameworkCore;

namespace PrayerTimes.Database
{
    public static class DbInitializer
    {
        public static void ApplyMigrations(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }
    }
}