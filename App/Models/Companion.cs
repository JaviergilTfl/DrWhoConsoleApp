namespace DrWhoConsoleApp.Models;

public partial class Companion
{
    public int CompanionId { get; set; }

    public string CompanionName { get; set; } = null!;

    public string? WhoPlayed { get; set; }

    public virtual ICollection<EpisodeCompanion> EpisodeCompanions { get; set; } = new List<EpisodeCompanion>();
}
