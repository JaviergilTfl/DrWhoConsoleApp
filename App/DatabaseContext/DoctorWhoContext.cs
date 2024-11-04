using DrWhoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrWhoConsoleApp.DatabaseContext;

public class DoctorWhoContext : DbContext, IDoctorWhoContext
{
    public DoctorWhoContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Companion> Companions { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Enemy> Enemies { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<EpisodeCompanion> EpisodeCompanions { get; set; }

    public virtual DbSet<EpisodeEnemy> EpisodeEnemies { get; set; }
}