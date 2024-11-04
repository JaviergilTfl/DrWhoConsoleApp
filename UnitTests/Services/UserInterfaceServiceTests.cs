using AutoFixture.NUnit3;
using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Models;
using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;
using Moq;

namespace UnitTests.Services
{
    public class UserInterfaceServiceTests
    {
        [Theory, AutoDomainData]
        public void Run_ShouldListDoctors_WhenChoiceIs1(
            Mock<IDoctorService> mockDoctorService,
            Mock<IConsoleService> mockConsoleService,
            List<Doctor> doctors,
            ListLogger<IUserInterfaceService> logger
            )
        {
            // Arrange
            mockConsoleService.SetupSequence(cs => cs.ReadLine())
                .Returns("1")
                .Returns("4"); // Exit after listing doctors

            mockDoctorService.Setup(ds => ds.GetAllDoctors()).Returns(doctors);

            var sut = new UserInterfaceService(mockDoctorService.Object, null, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            mockDoctorService.Verify(ds => ds.GetAllDoctors(), Times.Once);
            for (int i = 0; i < doctors.Count - 1; i++)
            {
                mockConsoleService.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains($"Doctor Name: {doctors[i].DoctorName}"))), Times.Exactly(1));
            }
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Error"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));
        }

        [Theory, AutoDomainData]
        public void Run_ShouldNotListMenuWhenConsoleThrowsException(
            Mock<IDoctorService> mockDoctorService,
            Mock<IConsoleService> mockConsoleService,
            ListLogger<IUserInterfaceService> logger,
            Exception exception
            )
        {
            // Arrange
            mockConsoleService.Setup(x => x.ReadLine()).Throws(new Exception(exception.Message));

            var sut = new UserInterfaceService(mockDoctorService.Object, null, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.WriteLine(It.IsAny<string>()), Times.Exactly(5));

            Assert.That(logger.Logs, Has.Exactly(1).Contains($"Error: An error occurred: {exception.Message}"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));
        }

        [Theory, AutoDomainData]
        public void Run_ShouldListEpisodesByYear_WhenChoiceIs2(
            Mock<IEpisodeService> mockEpisodeService,
            Mock<IConsoleService> mockConsoleService,
            List<Episode> episodes,
            ListLogger<IUserInterfaceService> logger)
        {
            // Arrange
            var year = episodes[0].EpisodeDate.ToString().Substring(6, 4);

            mockConsoleService.SetupSequence(cs => cs.ReadLine())
                .Returns("2")
                .Returns(year)
                .Returns("4"); // Exit after listing episodes

            mockEpisodeService.Setup(es => es.GetAllEpisodes()).Returns(episodes);

            var sut = new UserInterfaceService(null, mockEpisodeService.Object, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            mockEpisodeService.Verify(es => es.GetAllEpisodes(), Times.Once);
            mockConsoleService.Verify(cs => cs.Write($"Please enter the year you want to list the episodes for: "), Times.Exactly(1));

            for (int i = 0; i < episodes.Count - 1; i++)
            {
                mockConsoleService.Verify(cs => cs.WriteLine(
                    It.Is<string>(s => s.Contains($"{i + 1}. Episode: {episodes[i].Title}, Date: {episodes[i].EpisodeDate}, Doctor: {episodes[i].Doctor.DoctorName}")))
                , Times.Exactly(1));
            }
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Error"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));
        }

        [Theory, AutoDomainData]
        public void Run_ShouldAddNewDoctor_WhenChoiceIs3(
        Mock<IDoctorService> mockDoctorService,
        Mock<IConsoleService> mockConsoleService,
        ListLogger<IUserInterfaceService> logger,
        Doctor doctor)
        {
            // Arrange
            mockConsoleService.SetupSequence(cs => cs.ReadLine())
            .Returns("3")
            .Returns(doctor.DoctorName)
            .Returns(doctor.BirthDate.ToString())
            .Returns("4"); // Exit after adding doctor

            var sut = new UserInterfaceService(mockDoctorService.Object, null, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.Write(It.IsAny<string>()), Times.AtLeastOnce);
            mockConsoleService.Verify(cs => cs.ReadLine(), Times.AtLeastOnce);
            mockDoctorService.Verify(ds => ds.AddDoctor(It.IsAny<Doctor>()), Times.Once);
            mockConsoleService.Verify(cs => cs.WriteLine(
                It.Is<string>(s => s.Contains($"Doctor {doctor.DoctorName}"))), Times.Exactly(1));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Error"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));
        }

        [Theory, AutoDomainData]
        public void Run_ShouldExit_WhenChoiceIs4(
            Mock<IConsoleService> mockConsoleService,
            ListLogger<IUserInterfaceService> logger)
        {
            // Arrange
            mockConsoleService.Setup(cs => cs.ReadLine()).Returns("4");

            var sut = new UserInterfaceService(null, null, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            mockConsoleService.Verify(cs => cs.ReadLine(), Times.Once);
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Error"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));
        }

        [Theory, AutoDomainData]
        public void Run_ShouldHandleInvalidChoice(
            [Frozen] Mock<IConsoleService> mockConsoleService,
            //Mock<ILogger<IUserInterfaceService>> logger,
            ListLogger<IUserInterfaceService> logger,
            string choice)
        {
            // Arrange
            mockConsoleService.SetupSequence(cs => cs.ReadLine())
                .Returns(choice)
                .Returns("4");

            var sut = new UserInterfaceService(null, null, mockConsoleService.Object, logger);

            // Act
            sut.Run();

            // Assert
            mockConsoleService.Verify(cs => cs.WriteLine("Invalid choice. Please try again."), Times.Once);
            Assert.That(logger.Logs, Has.Exactly(1).Contains($"Warning: Invalid choice ({choice}) entered by the user"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Error"));
        }

        // TODO: add more tests for when AddDoctor throws an exception

        // TODO: add more tests for when ListEpisodesByYear: year entered is not valid
    }
}