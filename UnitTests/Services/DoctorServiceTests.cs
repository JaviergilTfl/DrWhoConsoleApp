using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Models;
using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;

namespace UnitTests.Services
{
    [TestFixture]
    public class DoctorServiceTests
    {
        [Theory, AutoDomainData]
        public async Task GetAllDoctors_ShouldReturnAllDoctors(
            DoctorWhoContext dbContext,
            List<Doctor> doctors)
        {
            // Arrange
            dbContext.Doctors.AddRange(doctors);
            dbContext.SaveChanges();

            var sut = new DoctorService(dbContext);

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
            Doctor doctor)
        {
            // Act
            var sut = new DoctorService(dbContext);

            sut.AddDoctor(doctor);

            // Assert
            dbContext.Doctors.Contains(doctor);
            dbContext.Doctors.Count().Equals(1);
        }
    }
}