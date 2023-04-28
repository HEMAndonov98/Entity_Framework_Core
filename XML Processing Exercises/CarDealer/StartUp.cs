using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            
            string suppliersInputXml = File.ReadAllText(GetFullPath("suppliers", ".xml"));

            Console.WriteLine(ImportSuppliers(context, suppliersInputXml));
        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static string GetFullPath(string DatasetName, string fileExtenstion)
        {
            string datasetWithExtension = $"{DatasetName}{fileExtenstion}";
            string defaultPath = "../../../Datasets/";
            var abstractPath = Path.Join(defaultPath, datasetWithExtension);

            return Path.GetFullPath(abstractPath);
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
                cfg.AddProfile<CarDealerProfile>()
            );

            IMapper mapper = mapperConfig.CreateMapper();

            return mapper;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Suppliers");

            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            var dtos = (ImportSupplierDto[])serializer.Deserialize(reader);

            List<Supplier> suppliers = dtos.Select(supp => mapper.Map<Supplier>(supp))
                .ToList();

            try
            {
                context.Suppliers.AddRange(suppliers);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            context.SaveChanges();
            
            return $"Successfully imported {suppliers.Count}";
        }
    }
}