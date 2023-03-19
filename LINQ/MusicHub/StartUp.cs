using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MusicHub
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();
            var albums = context.Albums
                .AsNoTracking()
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .Include(a => a.Producer)
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    Name = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("F2", CultureInfo.InvariantCulture),
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToList(),
                    AlbumPrice = a.Price
                })
                .ToList()
                .OrderByDescending(a => a.AlbumPrice);

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");
                int songNumber = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{songNumber++}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice.ToString("F2", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString()
                .Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sb = new StringBuilder();
            var songs = context.Songs
                .AsNoTracking()
                .Include(s => s.Album)
                .Include(s => s.Writer)
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .Where(s => s.Duration.Hours * 3600 + s.Duration.Minutes * 60 + s.Duration.Seconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => sp.Performer)
                        .Select(p => new {PerformerName = $"{p.FirstName} {p.LastName}"})
                        .ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                    
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToList();

            int songCount = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{songCount++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                foreach (var performer in song.Performers.OrderBy(p => p.PerformerName))
                {
                    sb.AppendLine($"---Performer: {performer.PerformerName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");
            }
            return sb.ToString()
                .Trim();
        }
    }
}
