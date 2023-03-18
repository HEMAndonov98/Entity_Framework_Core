
using MusicHub.Data.Models;

namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;

    public class MusicHubDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //Song
            
            builder.Entity<Song>()
                .Property(s => s.Price)
                .HasColumnType("DECIMAL(5,2)");
            
            //Performer
            
            builder.Entity<Performer>()
                .Property(p => p.NetWorth)
                .HasColumnType("Decimal(14,2)");
            
            //Producer
            
            builder.Entity<Producer>()
                .Property(p => p.Pseudonym)
                .HasColumnType("NVARCHAR");
            
            //Writer
            
            builder.Entity<Writer>()
                .Property(w => w.Pseudonym)
                .HasColumnType("NVARCHAR");
            
            //SongPerformer
            
                //Primary Key
            builder.Entity<SongPerformer>()
                .HasKey(sp => new { sp.SongId, sp.PerformerId });
    
                //Relationship
            builder.Entity<SongPerformer>()
                .HasOne(sp => sp.Song)
                .WithMany(s => s.SongPerformers);

            builder.Entity<SongPerformer>()
                .HasOne(sp => sp.Performer)
                .WithMany(p => p.PerformerSongs);
        }
    }
}
