using AutoFixture.NUnit3;
using DrWhoConsoleApp;
using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UnitTests
{
    public class StartUpTests
    {
        [Theory, AutoData]
        public void CreateHost_ShouldRegisterServices()
        {
            // Arrange
            var hostBuilder = StartUp.CreateHost();

            // Act
            var host = hostBuilder.Build();
            var serviceProvider = host.Services;

            // Assert
            Assert.NotNull(serviceProvider.GetService<IServiceRunner>());
            Assert.NotNull(serviceProvider.GetService<IDoctorWhoContext>());
            Assert.NotNull(serviceProvider.GetService<IConsoleService>());
            Assert.NotNull(serviceProvider.GetService<IDoctorService>());
            Assert.NotNull(serviceProvider.GetService<IEpisodeService>());
            Assert.NotNull(serviceProvider.GetService<IUserInterfaceService>());
        }

        [Theory, AutoData]
        public void CreateHost_ShouldConfigureLogging()
        {
            // Arrange
            var hostBuilder = StartUp.CreateHost();

            // Act
            var host = hostBuilder.Build();
            var loggerFactory = host.Services.GetService<ILoggerFactory>();

            // Assert
            Assert.NotNull(loggerFactory);
            var logger = loggerFactory.CreateLogger<StartUpTests>();
            Assert.NotNull(logger);
        }

        [Theory, AutoData]
        public void CreateHost_ShouldConfigureAppConfiguration()
        {
            // Arrange
            var hostBuilder = StartUp.CreateHost();

            // Act
            var host = hostBuilder.Build();
            var configuration = host.Services.GetService<IConfiguration>();

            // Assert
            Assert.NotNull(configuration);
            var value = configuration[Constants.AppSettings.DbConnectionString];
            Assert.NotNull(value);
        }
    }
}