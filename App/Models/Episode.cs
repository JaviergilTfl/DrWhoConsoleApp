namespace DrWhoConsoleApp.Models;

public partial class Episode
{
    public int EpisodeId { get; set; }

    public int? SeriesNumber { get; set; }

    public int? EpisodeNumber { get; set; }

    public string? EpisodeType { get; set; }

    public string? Title { get; set; }

    public DateOnly? EpisodeDate { get; set; }

    public int? AuthorId { get; set; }

    public int? DoctorId { get; set; }

    public string? Notes { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<EpisodeCompanion> EpisodeCompanions { get; set; } = new List<EpisodeCompanion>();

    public virtual ICollection<EpisodeEnemy> EpisodeEnemies { get; set; } = new List<EpisodeEnemy>();
}
