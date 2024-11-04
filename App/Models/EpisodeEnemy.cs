namespace DrWhoConsoleApp.Models;

public partial class EpisodeEnemy
{
    public int EpisodeEnemyId { get; set; }

    public int? EpisodeId { get; set; }

    public int? EnemyId { get; set; }

    public virtual Enemy? Enemy { get; set; }

    public virtual Episode? Episode { get; set; }
}