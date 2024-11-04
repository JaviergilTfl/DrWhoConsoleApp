using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;
using Moq;

namespace UnitTests.Services
{
    public class ServiceRunnerTests
    {
        [Theory, AutoDomainData]
        public async Task Run_ShouldCallUserInterfaceServiceRun(
            Mock<IUserInterfaceService> mockUserInterfaceService)
        {
            // Arrange
            var sut = new ServiceRunner(mockUserInterfaceService.Object);

            // Act
            await sut.Run();

            // Assert
            mockUserInterfaceService.Verify(x => x.Run(), Times.Once);
        }
    }
}