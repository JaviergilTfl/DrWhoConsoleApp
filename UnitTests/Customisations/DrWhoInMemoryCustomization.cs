using AutoFixture;
using DrWhoConsoleApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Disruptions.UnitTests.Common.Customisations
{
    public class DrWhoInMemoryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // Disruptions DB
            var dbContextOptions = new DbContextOptionsBuilder<DoctorWhoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging()
                .Options;

            var dbContext = new DoctorWhoContext(dbContextOptions);
            fixture.Register(() => dbContext);

            var dbDisruptionscontextMock = new Mock<DoctorWhoContext>(dbContextOptions);
            fixture.Register(() => dbDisruptionscontextMock);

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}