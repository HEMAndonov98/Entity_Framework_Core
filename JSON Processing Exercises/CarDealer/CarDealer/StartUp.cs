using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            string importJson = File.ReadAllText("../../../suppliers.json");
        }

        public static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var jSuppliers = JsonConvert.DeserializeObject<ImportSupplierDto>(inputJson);

            return $"Successfully imported {suppliers.Count}.";

        }
    }
}