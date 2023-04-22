using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();
            // ResetDatabase(context);
            //
            // string usersXmltoString = File.ReadAllText("../../../Datasets/users.xml");
            // string productsXmlString = File.ReadAllText("../../../Datasets/products.xml");
            // string categoryXmlString = File.ReadAllText("../../../Datasets/categories.xml");
            // string categoryProductXmlString = File.ReadAllText("../../../Datasets/categories-products.xml");
            //
            // Console.WriteLine(ImportUsers(context, usersXmltoString));
            // Console.WriteLine(ImportProducts(context, productsXmlString));
            // Console.WriteLine(ImportCategories(context, categoryXmlString));
            // Console.WriteLine(ImportCategoryProducts(context, categoryProductXmlString));

            XmlWriterSettings writerSettings = new XmlWriterSettings()
            {
                Indent = true
            };

            WriteProductsToResult(context, writerSettings);
        }

        private static void WriteProductsToResult(ProductShopContext context, XmlWriterSettings writerSettings)
        {
            XDocument document = XDocument.Parse(GetProductsInRange(context));
            string fileProductsPath = "../../../Results/Products.xml";

            if (File.Exists(fileProductsPath))
            {
                File.Delete(fileProductsPath);
            }

            using XmlWriter writer = XmlWriter.Create(fileProductsPath, writerSettings);
            document.WriteTo(writer);
        }

        public static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg => 
                cfg.AddProfile<ProductShopProfile>());

            var mapper = configuration.CreateMapper();

            return mapper;
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var root = new XmlRootAttribute("Users");
            var serializer = new XmlSerializer(typeof(ImportUserDto[]), root);

            ImportUserDto[] dtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                dtos = (ImportUserDto[])serializer.Deserialize(reader)!;
            }

            var users = dtos.Select(dto => mapper.Map<User>(dto))
                .ToList();
            
            if (users.Count != dtos.Length) throw new InvalidOperationException();
            context.Users.AddRange(users);

            context.SaveChanges();
            
            return $"Successfully imported {users.Count}";

        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
             var mapper = CreateMapper();
             var serialiser = new XmlSerializer(typeof(ImportProductDto));

             XElement[] products = XDocument.Parse(inputXml)
                .Root!
                .Elements()
                .ToArray();

            List<Product> validProducts = new List<Product>(); 
            foreach (var product in products)
            {
                int sellerId = int.Parse(product.Element("sellerId")!.Value);

                if (context.Users.Find(sellerId) == null)
                {
                    throw new InvalidOperationException();
                }

                ImportProductDto dto = (ImportProductDto)serialiser.Deserialize(product.CreateReader())!;
                validProducts.Add(mapper.Map<Product>(dto));
            }
            
            context.Products.AddRange(validProducts);
            context.SaveChanges();
            
            return $"Successfully imported {validProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var serialiser = new XmlSerializer(typeof(ImportCategoryDto));

            XElement[] xCategories = XDocument.Parse(inputXml)
                .Root!
                .Elements()
                .ToArray();

            List<Category> validCategories = new List<Category>();
            foreach (var xCategory in xCategories)
            {
                if (xCategory.Element("name") == null) continue;

                ImportCategoryDto categoryDto = (ImportCategoryDto)serialiser.Deserialize(xCategory.CreateReader())!;
                if (categoryDto == null) throw new InvalidDataException();
                
                validCategories.Add(mapper.Map<Category>(categoryDto));
            }
            
            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var mapper = CreateMapper();
            var serialiser = new XmlSerializer(typeof(ImportCategoryProductDto));

            XElement[] xCategoryProducts = XDocument.Parse(inputXml)
                .Root!
                .Elements()
                .ToArray();
            List<CategoryProduct> validCategoryProducts = new List<CategoryProduct>();
            foreach (XElement xCategoryProduct in xCategoryProducts)
            {
                ImportCategoryProductDto categoryProductDto =
                    (ImportCategoryProductDto)serialiser.Deserialize(xCategoryProduct.CreateReader())!;

                if (context.Categories.Find(categoryProductDto.CategoryId) == null ||
                    context.Products.Find(categoryProductDto.ProductId) == null) continue;
                
                validCategoryProducts.Add(mapper.Map<CategoryProduct>(categoryProductDto));
            }
            
            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();
            
            return $"Successfully imported {validCategoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var result = new StringBuilder();
            var config = CreateMapper().ConfigurationProvider;
            int productMinPrice = 500;
            int productMaxPrice = 1000;

            var productsInRange = context.Products
                .AsNoTracking()
                .Include(p => p.Buyer)
                .Where(p => p.Price >= productMinPrice &&
                            p.Price <= productMaxPrice)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductsExportDto>(config)
                .ToArray();

            var root = new XmlRootAttribute("Products");
            var serializer = new XmlSerializer(typeof(ProductsExportDto[]), root);

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, productsInRange);
                result.AppendLine(writer.ToString());
            }


            return result
                .ToString()
                .Trim();
        }
    }
}