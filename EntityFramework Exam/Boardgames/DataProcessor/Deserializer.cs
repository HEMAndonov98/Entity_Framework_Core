using System.Text;
using System.Xml.Serialization;
using Boardgames.DataProcessor.ImportDto;
using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using Data;
   
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var sb = new StringBuilder();
            
            ImportCreatorDto[] xmlCreators;
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Creators");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCreatorDto[]), rootAttribute);
            
            using (StringReader stringReader = new StringReader(xmlString))
            { 
                xmlCreators = (ImportCreatorDto[])serializer.Deserialize(stringReader);
            }

            var validCreators = new List<Creator>();

            foreach (ImportCreatorDto xmlCreator in xmlCreators)
            {
                if (!IsValid(xmlCreator))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validBoardgames = new List<Boardgame>();
                foreach (ImportBoardgameDto xmlBoardgame in xmlCreator.Boardgames)
                {
                    if (!IsValid(xmlBoardgame))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame newBoardgame = new Boardgame()
                    {
                        Name = xmlBoardgame.Name,
                        Rating = xmlBoardgame.Rating,
                        YearPublished = xmlBoardgame.YearPublished,
                        CategoryType = (CategoryType)xmlBoardgame.CategoryType,
                        Mechanics = xmlBoardgame.Mechanics
                    };
                    
                    validBoardgames.Add(newBoardgame);
                }

                Creator newCreator = new Creator()
                {
                    FirstName = xmlCreator.FirstName,
                    LastName = xmlCreator.LastName,
                    Boardgames = validBoardgames
                };

                validCreators.Add(newCreator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, newCreator.FirstName, newCreator.LastName,
                    newCreator.Boardgames.Count));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString()
                .Trim();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var jsonSellers = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);


            var validSellers = new List<Seller>();
            foreach (ImportSellerDto jsonSeller in jsonSellers)
            {
                if (!IsValid(jsonSeller))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Seller newSeller = new Seller()
                {
                    Name = jsonSeller.Name,
                    Address = jsonSeller.Address,
                    Country = jsonSeller.Country,
                    Website = jsonSeller.Website,
                };
                var esitingBoardgames = new List<Boardgame>();

                foreach (var boardgameId in jsonSeller.Boardgames.Distinct())
                {
                    var boardgame = context.Boardgames
                        .Find(boardgameId);

                    if (boardgame == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var newBoardGameSeller = new BoardgameSeller()
                    {
                        Seller = newSeller,
                        SellerId = newSeller.Id,
                        BoardgameId = boardgameId,
                        Boardgame = boardgame
                    };

                    newSeller.BoardgamesSellers.Add(newBoardGameSeller);
                    boardgame.BoardgamesSellers.Add(newBoardGameSeller);
                }
                
                validSellers.Add(newSeller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, newSeller.Name,
                    newSeller.BoardgamesSellers.Count));
            }
            
            context.Sellers.AddRange(validSellers);
            context.SaveChanges();

            return sb.ToString()
                .Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
