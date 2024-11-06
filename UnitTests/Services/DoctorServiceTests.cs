using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Models;
using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Moq;


namespace UnitTests.Services
{
    [TestFixture]
    public class DoctorServiceTests
    {
        [Theory, AutoDomainData]
        public async Task GetAllDoctors_ShouldReturnAllDoctors(
            DoctorWhoContext dbContext,
            Mock<ILogger<DoctorService>> logger,
            List<Doctor> doctors)
        {
            // Arrange
            dbContext.Doctors.AddRange(doctors);
            await dbContext.SaveChangesAsync();

            var sut = new DoctorService(dbContext, logger.Object);

            // Act
            var result = sut.GetAllDoctors();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(doctors.Count));
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.That(result.ElementAt(i).DoctorName, Is.EqualTo(doctors.ElementAt(i).DoctorName));
                Assert.That(result.ElementAt(i).DoctorNumber, Is.EqualTo(doctors.ElementAt(i).DoctorNumber));
                Assert.That(result.ElementAt(i).Episodes.Count, Is.EqualTo(doctors.ElementAt(i).Episodes.Count));
                Assert.That(result.ElementAt(i).DoctorId, Is.EqualTo(doctors.ElementAt(i).DoctorId));

                for (int j = 0; j < result.ElementAt(i).Episodes.Count; j++)
                {
                    Assert.That(result.ElementAt(i).Episodes.ElementAt(j).Title, Is.EqualTo(doctors.ElementAt(i).Episodes.ElementAt(j).Title));
                    Assert.That(result.ElementAt(i).Episodes.ElementAt(j).EpisodeNumber, Is.EqualTo(doctors.ElementAt(i).Episodes.ElementAt(j).EpisodeNumber));
                    Assert.That(result.ElementAt(i).Episodes.ElementAt(j).SeriesNumber, Is.EqualTo(doctors.ElementAt(i).Episodes.ElementAt(j).SeriesNumber));
                    Assert.That(result.ElementAt(i).Episodes.ElementAt(j).EpisodeId, Is.EqualTo(doctors.ElementAt(i).Episodes.ElementAt(j).EpisodeId));
                }
            }
        }

        [Theory, AutoDomainData]
        public async Task AddDoctor_ShouldAddDoctorToDatabase(
            DoctorWhoContext dbContext,
            Mock<ILogger<DoctorService>> logger,
            Doctor doctor)
        {
            // Act
            var sut = new DoctorService(dbContext, logger.Object);

            sut.AddDoctor(doctor);

            // Assert
            dbContext.Doctors.Contains(doctor);
            dbContext.Doctors.Count().Equals(1);

            Assert.NotNull(dbContext.Doctors.Find(doctor.DoctorId));
        }

        [Theory, AutoDomainData]
        public async Task AddDoctor_ShouldThrowExceptionWhenFailureOccursWhileAddingData(
            Mock<DoctorWhoContext> dbContext,
            ListLogger<DoctorService> logger,
            Doctor doctor,
            string exceptionMessage)
        {
            // Arrange
            var sut = new DoctorService(dbContext.Object, logger);
            dbContext.Setup(x => x.Doctors.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Throws(new DbUpdateException(exceptionMessage));

            // Act
            var result = Assert.ThrowsAsync<DbUpdateException>(() => sut.AddDoctor(doctor));

            // Assert
            result.Message.Should().Be(exceptionMessage);
            dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            logger.Logs.Should().Contain(x=>x.Contains(exceptionMessage));
        }

        [Theory, AutoDomainData]
        public async Task AddDoctor_ShouldThrowExceptionWhenFailureOccursWhileSavingData(
            Mock<DoctorWhoContext> dbContext,
            ListLogger<DoctorService> logger,
            string exceptionMessage,
            Doctor doctor)
        {
            // Arrange
            var sut = new DoctorService(dbContext.Object, logger);
            var mockSet = new Mock<DbSet<Doctor>>();

            dbContext.Setup(x => x.Doctors).Returns(mockSet.Object);
            mockSet.Setup(x => x.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                   .Returns(new ValueTask<EntityEntry<Doctor>>(Task.FromResult((EntityEntry<Doctor>)null)));
            dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException(exceptionMessage));

            // Act
            var result = Assert.ThrowsAsync<DbUpdateException>(() => sut.AddDoctor(doctor));

            // Assert
            result.Message.Should().Be(exceptionMessage);
            dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            dbContext.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            logger.Logs.Should().Contain(x => x.Contains(exceptionMessage));
        }
    }
}