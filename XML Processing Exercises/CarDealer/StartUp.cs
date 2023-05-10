using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            // ResetDatabase(context);
            //
            // string suppliersInputXml = File.ReadAllText(GetFullPath("suppliers", ".xml"));
            // string partsInputXml = File.ReadAllText(GetFullPath("parts", ".xml"));
            // string carsInputXml = File.ReadAllText(GetFullPath("cars", ".xml"));
            // string customerInputXml = File.ReadAllText(GetFullPath("customers", ".xml"));
            // string salesInputXml = File.ReadAllText(GetFullPath("sales", ".xml"));
            //
            // Console.WriteLine(ImportSuppliers(context, suppliersInputXml));
            // Console.WriteLine(ImportParts(context, partsInputXml));
            // Console.WriteLine(ImportCars(context, carsInputXml));
            // Console.WriteLine(ImportCustomers(context, customerInputXml));
            // Console.WriteLine(ImportSales(context, salesInputXml));

            // string carsXmlResult = GetCarsWithDistance(context);
            // WriteToDataset("cars", carsXmlResult);

            // string bmwXmlResult = GetCarsFromMakeBmw(context);
            // WriteToDataset("bmw-cars", bmwXmlResult);

            // string suppliersXmlResult = GetLocalSuppliers(context);
            // WriteToDataset("suppliers", suppliersXmlResult);

            // string carsWithPartsXmlResult = GetCarsWithTheirListOfParts(context);
            // WriteToDataset("cars-parts", carsWithPartsXmlResult);

            
            string customersXmlResult = GetTotalSalesByCustomer(context);
            WriteToDataset("customers", customersXmlResult);

        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static string GetFullPath(string datasetName, string fileExtenstion)
        {
            string datasetWithExtension = $"{datasetName}{fileExtenstion}";
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

        private static void WriteToDataset(string ResultName, string result)
        {
            var path = $"../../../Results/{ResultName}.xml";
            XDocument xDocument = XDocument.Parse(result);
            var settings = new XmlWriterSettings()
            {
                Indent = true
            };

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            using XmlWriter xmlWriter = XmlWriter.Create(path, settings);
            xDocument.WriteTo(xmlWriter);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Suppliers");

            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            var dtos = (ImportSupplierDto[])serializer.Deserialize(reader)!;

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

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Parts");

            var serializer = new XmlSerializer(typeof(ImportPartDto[]), root);
            using StringReader reader = new StringReader(inputXml);

            var dtos = (ImportPartDto[])serializer.Deserialize(reader)!;

            List<Part> validParts = new();

            foreach (var dto in dtos)
            {
                if (context.Suppliers.Find(dto.SupplierId) != null)
                {
                    Part newPart = mapper.Map<Part>(dto);
                    validParts.Add(newPart);
                }
            }

            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Cars");

            var serializer = new XmlSerializer(typeof(ImportCarDto[]), root);
            using StringReader reader = new StringReader(inputXml);

            var dtos = (ImportCarDto[])serializer.Deserialize(reader)!;

            List<Car> validCars = new();
            foreach (var dto in dtos)
            {
                Car newCar = mapper.Map<Car>(dto);

                for (int i = 0; i < dto.Parts.Length; i++)
                {
                    if (context.Parts.Find(dto.Parts[i].Id) != null &&
                        newCar.PartsCars.FirstOrDefault(pc => pc.PartId == dto.Parts[i].Id) == null)
                    {
                        newCar.PartsCars.Add(new PartCar()
                        {
                            Car = newCar,
                            PartId = dto.Parts[i].Id
                        });
                    }
                }
                validCars.Add(newCar);
            }
            
            context.Cars.AddRange(validCars);
            context.SaveChanges();
            return $"Successfully imported {validCars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Customers");

            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), root);
            using StringReader reader = new StringReader(inputXml);

            var dtos = (ImportCustomerDto[])serializer.Deserialize(reader)!;

            List<Customer> customers = new();
            customers.AddRange(dtos.Select(dto => mapper.Map<Customer>(dto)));
            
            context.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Sales");

            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), root);
            using StringReader reader = new StringReader(inputXml);

            var dtos = (ImportSaleDto[])serializer.Deserialize(reader)!;

            List<Sale> validSales = new();
            for (int i = 0; i < dtos.Length; i++)
            {
                if (context.Cars.Find(dtos[i].CarId) != null)
                {
                    validSales.Add(mapper.Map<Sale>(dtos[i]));
                }
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();
            return $"Successfully imported {validSales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var cars = context.Cars
                .AsNoTracking()
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarDto>(config)
                .ToArray();

            var root = new XmlRootAttribute("Cars");
            var serializer = new XmlSerializer(typeof(ExportCarDto[]) ,root);

            using StringWriter writer = new();
            
            serializer.Serialize(writer, cars);

            return writer.ToString()
                .Trim();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var root = new XmlRootAttribute("cars");

            var serializer = new XmlSerializer(typeof(ExportBmwCarDto[]), root);

            var bmwCars = context.Cars
                .AsNoTracking()
                .Where(c => c.Make.ToLower() == "bmw")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportBmwCarDto>(config)
                .ToArray();

            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, bmwCars);
            
            return writer.ToString()
                .Trim();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var root = new XmlRootAttribute("suppliers");

            var suppliers = context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportSupplierDto>(config)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSupplierDto[]), root);
            using StringWriter writer = new StringWriter();
            
            serializer.Serialize(writer, suppliers);

            return writer.ToString()
                .Trim();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var root = new XmlRootAttribute("cars");

            var cars = context.Cars
                .Include(c => c.PartsCars)
                .ThenInclude(pc => pc.Part)
                .AsNoTracking()
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarsWithPartsDto>(config)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithPartsDto[]), root);
            using StringWriter writer = new();
            
            serializer.Serialize(writer, cars);
            return writer.ToString()
                .Trim();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var root = new XmlRootAttribute("customers");

            var customers = context.Customers
                .Include(c => c.Sales)
                .ThenInclude(s => s.Car)
                .ThenInclude(c => c.PartsCars)
                .ThenInclude(pc => pc.Part)
                .AsNoTracking()
                .Where(c => c.Sales.Count >= 1)
                .ProjectTo<ExportCustomerDto>(config)
                .AsEnumerable()
                .OrderByDescending(dto => dto.SpentMoney)
                .ToArray();
            
            var serializer = new XmlSerializer(typeof(ExportCustomerDto[]), root);
            using StringWriter writer = new StringWriter();
            
            serializer.Serialize(writer, customers);

            return writer.ToString()
                .Trim();
        }
    }
}