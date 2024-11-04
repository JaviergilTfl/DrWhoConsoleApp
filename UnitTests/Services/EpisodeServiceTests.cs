using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Models;
using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace UnitTests.Services
{
    public class EpisodeServiceTests
    {
        [Theory, AutoDomainData]
        public void GetAllEpisodes_ShouldReturnAllEpisodes(
            DoctorWhoContext dbContext,
            List<Episode> episodes,
            Mock<ILogger<EpisodeService>> logger
            )
        {
            // Arrange
            dbContext.Episodes.AddRange(episodes);
            dbContext.SaveChanges();

            var sut = new EpisodeService(dbContext, logger.Object);

            // Act
            var result = sut.GetAllEpisodes();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(episodes.Count));
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.That(result.ElementAt(i).Title, Is.EqualTo(episodes.ElementAt(i).Title));
                Assert.That(result.ElementAt(i).EpisodeNumber, Is.EqualTo(episodes.ElementAt(i).EpisodeNumber));
                Assert.That(result.ElementAt(i).SeriesNumber, Is.EqualTo(episodes.ElementAt(i).SeriesNumber));
                Assert.That(result.ElementAt(i).EpisodeId, Is.EqualTo(episodes.ElementAt(i).EpisodeId));

                Assert.That(result.ElementAt(i).Doctor.DoctorName, Is.EqualTo(episodes.ElementAt(i).Doctor.DoctorName));
                Assert.That(result.ElementAt(i).Doctor.DoctorNumber, Is.EqualTo(episodes.ElementAt(i).Doctor.DoctorNumber));
                Assert.That(result.ElementAt(i).Doctor.Episodes.Count, Is.EqualTo(episodes.ElementAt(i).Doctor.Episodes.Count));
                Assert.That(result.ElementAt(i).Doctor.DoctorId, Is.EqualTo(episodes.ElementAt(i).Doctor.DoctorId));

                for (int j = 0; j < result.ElementAt(i).Doctor.Episodes.Count; j++)
                {
                    Assert.That(result.ElementAt(i).Doctor.Episodes.ElementAt(j).Title, Is.EqualTo(episodes.ElementAt(i).Doctor.Episodes.ElementAt(j).Title));
                    Assert.That(result.ElementAt(i).Doctor.Episodes.ElementAt(j).EpisodeNumber, Is.EqualTo(episodes.ElementAt(i).Doctor.Episodes.ElementAt(j).EpisodeNumber));
                    Assert.That(result.ElementAt(i).Doctor.Episodes.ElementAt(j).SeriesNumber, Is.EqualTo(episodes.ElementAt(i).Doctor.Episodes.ElementAt(j).SeriesNumber));
                    Assert.That(result.ElementAt(i).Doctor.Episodes.ElementAt(j).EpisodeId, Is.EqualTo(episodes.ElementAt(i).Doctor.Episodes.ElementAt(j).EpisodeId));
                }

                Assert.That(result.ElementAt(i).Author.AuthorName, Is.EqualTo(episodes.ElementAt(i).Author.AuthorName));
                Assert.That(result.ElementAt(i).Author.AuthorId, Is.EqualTo(episodes.ElementAt(i).Author.AuthorId));

                for (int j = 0; j < result.ElementAt(i).EpisodeCompanions.Count; j++)
                {
                    Assert.That(result.ElementAt(i).EpisodeCompanions.ElementAt(j).Companion, Is.EqualTo(episodes.ElementAt(i).EpisodeCompanions.ElementAt(j).Companion));
                    Assert.That(result.ElementAt(i).EpisodeCompanions.ElementAt(j).CompanionId, Is.EqualTo(episodes.ElementAt(i).EpisodeCompanions.ElementAt(j).CompanionId));
                }

                for (int j = 0; j < result.ElementAt(i).EpisodeEnemies.Count; j++)
                {
                    Assert.That(result.ElementAt(i).EpisodeEnemies.ElementAt(j).Enemy, Is.EqualTo(episodes.ElementAt(i).EpisodeEnemies.ElementAt(j).Enemy));
                    Assert.That(result.ElementAt(i).EpisodeEnemies.ElementAt(j).EpisodeEnemyId, Is.EqualTo(episodes.ElementAt(i).EpisodeEnemies.ElementAt(j).EpisodeEnemyId));
                }
            }
        }

        [Theory, AutoDomainData]
        public async Task AddEpisode_ShouldAddEpisodeToDatabase(
            DoctorWhoContext dbContext,
            Episode episode,
            Mock<ILogger<EpisodeService>> logger)
        {
            // Act
            var sut = new EpisodeService(dbContext, logger.Object);

            sut.AddEpisode(episode);

            // Assert
            dbContext.Episodes.Contains(episode);
            dbContext.Episodes.Count().Equals(1);

            for (int i = 0; i < episode.EpisodeCompanions.Count; i++)
            {
                dbContext.EpisodeCompanions.Contains(episode.EpisodeCompanions.ElementAt(i));
                dbContext.EpisodeCompanions.Count().Equals(1);
            }

            for (int i = 0; i < episode.EpisodeEnemies.Count; i++)
            {
                dbContext.EpisodeEnemies.Contains(episode.EpisodeEnemies.ElementAt(i));
                dbContext.EpisodeEnemies.Count().Equals(1);
            }

            dbContext.Authors.Contains(episode.Author);
            dbContext.Authors.Count().Equals(1);

            dbContext.Doctors.Contains(episode.Doctor);
            dbContext.Doctors.Count().Equals(1);
        }

        [Theory, AutoDomainData]
        public void AddEpisode_ShouldNotBeAddedWhenExceptionIsThrown(
            DoctorWhoContext dbContext,
            Episode episode,
            ListLogger<EpisodeService> logger)
        {
            // Act
            var sut = new EpisodeService(dbContext, logger);

            var episodeText = JsonSerializer.Serialize(episode);

            dbContext.Dispose();
            sut.AddEpisode(episode);

            Assert.Throws<ObjectDisposedException>(() => dbContext.Episodes.Contains(episode));
            logger.Logs.Count().Equals(1);
            Assert.That(logger.Logs, Has.Exactly(1).Contains($"Error"));
            Assert.That(logger.Logs, Has.Exactly(1).Contains(episodeText));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Information"));
            Assert.That(logger.Logs, Has.Exactly(0).Contains($"Warning"));

        }

        [Theory, AutoDomainData]
        public async Task RemoveEpisode_ShouldRemoveEpisodeFromDatabase(
            DoctorWhoContext dbContext,
            Episode episode,
            Mock<ILogger<EpisodeService>> logger
            )
        {
            // Arrange
            dbContext.Episodes.Add(episode);
            dbContext.SaveChanges();

            // Act
            var sut = new EpisodeService(dbContext, logger.Object);

            sut.RemoveEpisode(episode);

            // Assert
            dbContext.Episodes.Contains(episode).Equals(false);
            dbContext.Episodes.Count().Equals(0);

            for (int i = 0; i < episode.EpisodeCompanions.Count; i++)
            {
                dbContext.EpisodeCompanions.Contains(episode.EpisodeCompanions.ElementAt(i)).Equals(false);
                dbContext.EpisodeCompanions.Count().Equals(0);
            }

            for (int i = 0; i < episode.EpisodeEnemies.Count; i++)
            {
                dbContext.EpisodeEnemies.Contains(episode.EpisodeEnemies.ElementAt(i)).Equals(false);
                dbContext.EpisodeEnemies.Count().Equals(0);
            }

            dbContext.Authors.Contains(episode.Author).Equals(false);
            dbContext.Authors.Count().Equals(0);

            dbContext.Doctors.Contains(episode.Doctor).Equals(false);
            dbContext.Doctors.Count().Equals(0);
        }
    }
}