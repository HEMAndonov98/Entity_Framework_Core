﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext db = new CarDealerContext();
            //ResetDatabase(db);
            
            // string importSupplierJson = File.ReadAllText("../../../Datasets/suppliers.json");
            // string importPartJson = File.ReadAllText("../../../Datasets/parts.json");
            // string importCarJson = File.ReadAllText("../../../Datasets/cars.json");
            // string importCustomerJson = File.ReadAllText("../../../Datasets/customers.json");
            // string importSaleJson = File.ReadAllText("../../../Datasets/sales.json");
            //
            //
            // Console.WriteLine(ImportSuppliers(db, importSupplierJson));
            // Console.WriteLine(ImportParts(db, importPartJson));
            // Console.WriteLine(ImportCars(db, importCarJson));
            // Console.WriteLine(ImportCustomers(db, importCustomerJson));
            // Console.WriteLine(ImportSales(db, importSaleJson));

            //Console.WriteLine(GetOrderedCustomers(db));
            //Console.WriteLine(GetCarsFromMakeToyota(db));
            Console.WriteLine(GetLocalSuppliers(db));
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

        //Import    

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

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            var customerDtos = JsonConvert.DeserializeObject<List<ImportCustomerDto>>(inputJson);
            var newCustomers = customerDtos!
                .Select(c => mapper.Map<Customer>(c))
                .ToList();
            
            context.Customers.AddRange(newCustomers);
            context.SaveChanges();
            
            return $"Successfully imported {newCustomers.Count}."; ;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            List<ImportSaleDto> saleDtos = JsonConvert.DeserializeObject<List<ImportSaleDto>>(inputJson);

            List<Sale> validSales = new List<Sale>();
            foreach (ImportSaleDto saleDto in saleDtos)
            {
                if (context.Cars.Find(saleDto.CarId) == null ||
                    context.Customers.Find(saleDto.CustomerId) == null) continue;

                Sale newSale = mapper.Map<Sale>(saleDto);
                validSales.Add(newSale);
            }
            
            context.Sales.AddRange(validSales);
            context.SaveChanges();
            return $"Successfully imported {validSales.Count}.";
        }
        
        //Export

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            var customers = context.Customers
                .AsNoTracking()
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.IsYoungDriver)
                .ProjectTo<ExportCustomerDto>(config)
                .ToList();

            var customersJson = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return customersJson;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;
            string searchedMake = "Toyota";

            var cars = context.Cars
                .AsNoTracking()
                .Where(c => c.Make == searchedMake)
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportCarDto>(config)
                .ToList();

            var carsJson = JsonConvert.SerializeObject(cars, Formatting.Indented);
            
            return carsJson;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var config = CreateMapper().ConfigurationProvider;

            var localSuppliers = context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportSupplierDto>(config)
                .ToList();

            var suppliersJson = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            return suppliersJson;
        }
    }
}