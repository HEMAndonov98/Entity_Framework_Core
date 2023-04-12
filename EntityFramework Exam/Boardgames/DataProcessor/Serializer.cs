using Boardgames.DataProcessor.ExportDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            return string.Empty;
            throw new NotImplementedException();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .AsNoTracking()
                .Include(s => s.BoardgamesSellers)
                .ThenInclude(bs => bs.Boardgame)
                .Where(s => s.BoardgamesSellers
                    .Select(bs => bs.Boardgame)
                    .Any(b => b.YearPublished >= year && b.Rating <= rating)
                )
                .Select(s => new ExportSellerDto()
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Select(bs => bs.Boardgame)
                        .Where(b => b.YearPublished >= year &&
                                    b.Rating <= rating)
                        .Select(b => new ExportBoardgameDto()
                        {
                            Name = b.Name,
                            Rating = b.Rating,
                            CategoryType = b.CategoryType.ToString(),
                            Mechanics = b.Mechanics
                        })
                        .OrderByDescending(bDto => bDto.Rating)
                        .ThenBy(bDto => bDto.Name)
                        .ToList()
                })
                .ToList()
                .OrderByDescending(sDto => sDto.Boardgames.Count())
                .ThenBy(sDto => sDto.Name);

            string jsonString = JsonConvert.SerializeObject(sellers);
            return jsonString;
        }
    }
}