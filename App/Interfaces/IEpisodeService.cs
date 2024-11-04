using DrWhoConsoleApp.Models;

namespace DrWhoConsoleApp.Interfaces
{
    public interface IEpisodeService
    {
        void AddEpisode(Episode episode);
        IEnumerable<Episode> GetAllEpisodes();
        void RemoveEpisode(Episode episode);
    }
}
