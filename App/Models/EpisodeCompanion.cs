namespace DrWhoConsoleApp.Models;

public partial class EpisodeCompanion
{
    public int EpisodeCompanionId { get; set; }

    public int? EpisodeId { get; set; }

    public int? CompanionId { get; set; }

    public virtual Companion? Companion { get; set; }

    public virtual Episode? Episode { get; set; }
}
