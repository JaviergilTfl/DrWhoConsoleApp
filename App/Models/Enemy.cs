namespace DrWhoConsoleApp.Models;

public partial class Enemy
{
    public int EnemyId { get; set; }

    public string? EnemyName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<EpisodeEnemy> EpisodeEnemies { get; set; } = new List<EpisodeEnemy>();
}
