using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using var db = new ProductShopContext();
            InitialiseDb(db);
            
            string jsonImportUsers = File.ReadAllText(@"../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(db, jsonImportUsers));
            Console.WriteLine(ImportProducts());
        }

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
            int countAdded = newUsers.Count;
            return $"Successfully imported {countAdded}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            return string.Empty;
        }
    }
}