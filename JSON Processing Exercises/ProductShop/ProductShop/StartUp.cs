using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

using Castle.Core.Internal;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ProductShop
{
        using Data;
        using Models;
        using DTOs.Export;
        using DTOs.Import;
    public class StartUp
    {
        
        public static void Main()
        {
            using var db = new ProductShopContext();
            // InitialiseDb(db);
            //
            // string jsonImportUsers = File.ReadAllText(@"../../../Datasets/users.json");
            // string jsonImportProducts = File.ReadAllText(@"../../../Datasets/products.json");
            // string jsonImportCategories = File.ReadAllText(@"../../../Datasets/categories.json");
            // string jsonImportCategoriesProducts = File.ReadAllText(@"../../../Datasets/categories-products.json");
            //
            //
            //
            // Console.WriteLine(ImportUsers(db, jsonImportUsers));
            // Console.WriteLine(ImportProducts(db, jsonImportProducts));
            // Console.WriteLine(ImportCategories(db, jsonImportCategories));
            // Console.WriteLine(ImportCategoryProducts(db, jsonImportCategoriesProducts));

            //Console.WriteLine(GetProductsInRange(db));
            //Console.WriteLine(GetSoldProducts(db));
            //Console.WriteLine(GetCategoriesByProductsCount(db));
            Console.WriteLine(GetUsersWithProducts(db));
        }
        //Import Data

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ProductShopProfile()));
            var mapper = config.CreateMapper();
            return mapper;
        }

        public static void InitialiseDb(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("ProductShop Database successfully created");
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            JArray jObojects = JArray.Parse(inputJson);
            HashSet<User> newUsers = new HashSet<User>();
            
            foreach (var jobject in jObojects)
            {
                ImportUserDto? newUserDto = JsonConvert.DeserializeObject<ImportUserDto>(jobject.ToString());

                if (newUserDto == null)
                {
                    throw new InvalidOperationException();
                }
                
                User user = mapper.Map<User>(newUserDto);
                newUsers.Add(user);
            }

            context.Users.AddRange(newUsers);
            context.SaveChanges();
            int countAdded = newUsers.Count;
            return $"Successfully imported {countAdded}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            JArray jproducts = JArray.Parse(inputJson);
            HashSet<Product> products = new HashSet<Product>();

            foreach (var jproduct in jproducts)
            {
                ImportProductDto? newDto = JsonConvert.DeserializeObject<ImportProductDto>(jproduct.ToString());

                Product newProduct = mapper.Map<Product>(newDto);
                products.Add(newProduct);
            }
            
            context.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var settintgs = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            
            IMapper mapper = CreateMapper();
            JArray jcategories = JArray.Parse(inputJson);

            HashSet<Category> categories = new HashSet<Category>();
            foreach (JToken jcategory in jcategories)
            {
                ImportCategoryDto? newDto = JsonConvert.DeserializeObject<ImportCategoryDto>(jcategory.ToString(), settintgs);

                if (newDto.Name.IsNullOrEmpty()) continue;

                    Category newCategory = mapper.Map<Category>(newDto);
                categories.Add(newCategory);
            }
            
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            JArray jcategoryProducts = JArray.Parse(inputJson);
            HashSet<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

            foreach (JToken jcategoryProduct in jcategoryProducts)
            {
                ImportCategoryProductDto? newDto = JsonConvert.DeserializeObject<ImportCategoryProductDto>(
                    jcategoryProduct.ToString());

                CategoryProduct newCategoryProduct = mapper.Map<CategoryProduct>(newDto);
                categoryProducts.Add(newCategoryProduct);
            }
            
            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";

        }
        
        //Export Data

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var products = context.Products
                .AsNoTracking()
                .Include(p => p.Seller)
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductsInRangeDto>(mapper.ConfigurationProvider)
                .ToList();

            var jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);
            
            return jsonProducts;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0007: Large number of DB records", MessageId = "count: 200")]
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var userProductsDto = context.Users
                .AsNoTracking()
                .Include(u => u.ProductsSold)
                .ThenInclude(ps => ps.Buyer)
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUsersSoldProductsDto>(mapper.ConfigurationProvider)
                .ToList();
            
            
            var json = JsonConvert.SerializeObject(userProductsDto, Formatting.Indented);
            
            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var categoryDtos = context.Categories
                .AsNoTracking()
                .Include(c => c.CategoriesProducts)
                .ThenInclude(cp => cp.Product)
                .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
                .ToList()
                .OrderByDescending(dto => dto.ProductsCount);

            var json = JsonConvert.SerializeObject(categoryDtos, Formatting.Indented);
         return   json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //I couldn't figure it out how to do this using automapper, I would love to revisit this some day or have someone explain it better to me
            
            var usersDtos = context.Users
                .AsNoTracking()
                .Include(u => u.ProductsSold)
                .ThenInclude(ps => ps.Buyer)
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                        {
                            count = u.ProductsSold.Count(p => p.Buyer != null),
                            products = u.ProductsSold
                                .Where(p => p.Buyer != null)
                                .Select(ps => new
                                {
                                    name = ps.Name,
                                    price = ps.Price
                                })
                                .OrderByDescending(p => u.ProductsSold
                                    .Count(p => p.Buyer != null))
                                .ToList()
                        }
                    })
                    .AsSplitQuery()
                .ToList();

            var usersWrapper = new
            {
                usersCount = usersDtos.Count,
                users = usersDtos
            };

            
            var serializerSettings = new JsonSerializerSettings() 
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(usersWrapper, Formatting.Indented, serializerSettings);
            return json;
        }
    }
}