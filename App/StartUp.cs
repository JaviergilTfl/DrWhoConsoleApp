using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DrWhoConsoleApp
{
    public class StartUp
    {
        public static IHostBuilder CreateHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appSettings.json");
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);

                    // Configure Serilog: logging would normally not go to a file but to a logging service like Azure App Insights
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(context.Configuration)
                        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    logging.AddSerilog();
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient<IServiceRunner, ServiceRunner>();

                    services.AddTransient(RegisterDbContext);

                    services.AddTransient<IConsoleService, ConsoleService>();
                    services.AddTransient<IDoctorService, DoctorService>();
                    services.AddTransient<IEpisodeService, EpisodeService>();
                    services.AddTransient<IUserInterfaceService, UserInterfaceService>();
                });
        }

        private static Func<IServiceProvider, IDoctorWhoContext> RegisterDbContext => provider =>
        {
            string connectionString = provider.GetService<IConfiguration>().GetValue<string>(Constants.AppSettings.DbConnectionString);

            var options = new DbContextOptionsBuilder<DoctorWhoContext>()
            .UseSqlServer(connectionString)
            .Options;
            return new DoctorWhoContext(options);
        };
    }
}