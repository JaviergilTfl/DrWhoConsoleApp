using DrWhoConsoleApp.Models;

namespace DrWhoConsoleApp.Interfaces
{
    public interface IEpisodeService
    {
        Task<int> AddEpisode(Episode episode);
        IEnumerable<Episode> GetAllEpisodes();
        void RemoveEpisode(Episode episode);
    }
}
