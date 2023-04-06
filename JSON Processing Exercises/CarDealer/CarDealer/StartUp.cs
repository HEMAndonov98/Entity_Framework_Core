using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext db = new CarDealerContext();
            ResetDatabase(db);
            
            string importSupplierJson = File.ReadAllText("../../../Datasets/suppliers.json");
            string importPartJson = File.ReadAllText("../../../Datasets/parts.json");
            string importCarJson = File.ReadAllText("../../../Datasets/cars.json");
            
            
            Console.WriteLine(ImportSuppliers(db, importSupplierJson));
            Console.WriteLine(ImportParts(db, importPartJson));
            Console.WriteLine(ImportCars(db, importCarJson));
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
        public static bool HasValidParts(int[] ids, CarDealerContext context)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (context.Parts.Find(ids[i]) == null)
                {
                    return false;
                }
            }

            return true;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            
            List<ImportSupplierDto>? jSuppliers = JsonConvert.DeserializeObject<List<ImportSupplierDto>>(inputJson)!;
            List<Supplier> suppliers = jSuppliers.Select(supp => mapper.Map<Supplier>(supp)).ToList();
            
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";

        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            
            ImportPartDto[]? partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            List<Part> validParts = new List<Part>();
            foreach (ImportPartDto partDto in partDtos!)
            {
                if (context.Suppliers.Find(partDto.SupplierId) == null) continue;

                validParts.Add(mapper.Map<Part>(partDto));
            }
            
            context.Parts.AddRange(validParts);
            context.SaveChanges();
            
            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            ImportCarDto[]? carDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            List<Car> validCars = new List<Car>();
            foreach (var dto in carDtos!)
            {
                Car newCar = mapper.Map<Car>(dto);

                if (!HasValidParts(dto.PartIds, context)) continue;

                 foreach (var partId in dto.PartIds)
                 {
                     if (newCar.PartsCars
                         .Any(pc => pc.PartId == partId)) continue;

                         var newPart = new PartCar()
                         {
                             PartId = partId
                         };
                     newCar.PartsCars.Add(newPart);
                 }
                validCars.Add(newCar);
            }

            context.AddRange(validCars);
            context.SaveChanges();
            
            return $"Successfully imported {validCars.Count}.";
        }

        
    }
}