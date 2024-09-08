using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrayerTimes.Database;
using PrayerTimes.Helper;
namespace PrayerTimes
{
    internal static class Program
    {
        [STAThread]
         static void Main()
        {
            StartUp.SetStartup(true);
            var services = new ServiceCollection();
            ConfigureServices(services);

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    DbInitializer.ApplyMigrations(dbContext);
                }
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                //ApplicationConfiguration.Initialize();
                var mainForm = serviceProvider.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            var connString = "Data Source=Database.db";
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connString));

            services.AddTransient<MainForm>();
        }

    }
}