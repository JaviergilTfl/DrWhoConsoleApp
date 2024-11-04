using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DrWhoConsoleApp.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IDoctorWhoContext _context;
        private readonly ILogger<EpisodeService> _logger;

        public EpisodeService(IDoctorWhoContext context, ILogger<EpisodeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddEpisode(Episode episode)
        {
            try
            {
                _context.Episodes.Add(episode);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var episodeText = JsonSerializer.Serialize(episode);

                var message = $"Error occurred while adding a new episode with details: {episodeText}\n" +
                    $"Error: {ex.Message}";
                _logger.Log(LogLevel.Error, message);
            }
        }


        public void RemoveEpisode(Episode episode)
        {
            _context.Episodes.Remove(episode);
            _context.SaveChanges();
        }

        public IEnumerable<Episode> GetAllEpisodes()
        {
            return _context.Episodes
                         .Include(e => e.Doctor)
                         .Include(e => e.Author)
                         .ToList();
        }
    }
}