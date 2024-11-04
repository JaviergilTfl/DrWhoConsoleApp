using DrWhoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrWhoConsoleApp.DatabaseContext
{
    public interface IDoctorWhoContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Companion> Companions { get; set; }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Enemy> Enemies { get; set; }
        DbSet<EpisodeCompanion> EpisodeCompanions { get; set; }
        DbSet<EpisodeEnemy> EpisodeEnemies { get; set; }
        DbSet<Episode> Episodes { get; set; }
        int SaveChanges();
    }
}