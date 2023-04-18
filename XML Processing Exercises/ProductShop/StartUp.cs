using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();
            ResetDatabase(context);

            string usersXmltoString = File.ReadAllText("../../../Datasets/users.xml");
            string productsXmlString = File.ReadAllText("../../../Datasets/products.xml");
            string categoryXmlString = File.ReadAllText("../../../Datasets/categories.xml");

            Console.WriteLine(ImportUsers(context, usersXmltoString));
            Console.WriteLine(ImportProducts(context, productsXmlString));
            Console.WriteLine(ImportCategories(context, categoryXmlString));
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

             XElement[] products = XDocument.Parse(inputXml)!
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
    }
}