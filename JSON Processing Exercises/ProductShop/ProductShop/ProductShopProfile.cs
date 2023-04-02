using AutoMapper;
using Microsoft.Data.SqlClient;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import
            
            //User Profile
            this.CreateMap<ImportUserDto, User>();
            
            //Product Profile
            this.CreateMap<ImportProductDto, Product>();
            
            //Category Profile
            this.CreateMap<ImportCategoryDto, Category>();
            
            //CategoryProduct Profile
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
            
            //Export
            
            //ProductsInRange
            this.CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(dst => dst.SellerFullName, opt => opt
                    .MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}".Trim()));
            
            //User Sold Products
            
                    //I am leaving this for the future me. When you want to use automapper and you need to map an Entity to a Dto but this dto needs to have a collection of another entity mirroring the real entity what I need to specify in the configuration is 
                    //1. How I want the outer entity to map to my outer Dto
                    //2. Inside this dto how I want the collection to map to the collection Ei Collection of Product Entity to Collection of ProductDto
                    //3. Inside this I will specify how I want a single Entity of type Product to map to a single type of ProductDto
                    //4. Automapper will then know that I want to map a collection to another collection and how to map a singular element from the collection
            CreateMap<Product, ExportSoldProductDto>()
                .ForMember(dest => dest.BuyerFirstName, opt => opt.MapFrom(src => src.Buyer.FirstName))
                .ForMember(dest => dest.BuyerLastName, opt => opt.MapFrom(src => src.Buyer.LastName));


            CreateMap<User, ExportUsersSoldProductsDto>()
                .ForMember(dest => dest.ProductsSold, opt => opt
                    .MapFrom(src => src.ProductsSold));
            
            this.CreateMap<ICollection<Product>, List<ExportSoldProductDto>>();


            //Category Export Dto
            
            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(dst => dst.CategoryName, opt => opt
                    .MapFrom(src => src.Name))
                .ForMember(dst => dst.ProductsCount, opt => opt
                    .MapFrom(src => src.CategoriesProducts.Count))
                .ForMember(dst => dst.AveragePrice, opt => opt
                    .MapFrom(src => src.CategoriesProducts
                        .Select(cp => cp.Product.Price).Average().ToString("F2")))
                .ForMember(dst => dst.TotalRevenue, opt => opt
                    .MapFrom(src => src.CategoriesProducts
                        .Select(cp => cp.Product.Price).Sum().ToString("F2")));
            
            //Export Users with Products

            this.CreateMap<Product, ExportProductDto>()
                .ForMember(dst => dst.Name, opt => opt
                    .MapFrom(src => src.Name))
                .ForMember(dst => dst.Price, opt => opt
                    .MapFrom(src => src.Price));

            this.CreateMap<Product, ICollection<ExportProductDto>>();


            this.CreateMap<User, ExportUsersDto>()
                .ForMember(dst => dst.FirstName, opt => opt
                    .MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, opt => opt
                    .MapFrom(src => src.LastName))
                .ForMember(dst => dst.Products, opt => opt
                    .MapFrom(src => src.ProductsSold));

            this.CreateMap<User, ICollection<ExportUsersDto>>();

            this.CreateMap<User, ExportUsersWithProductsDto>()
                .ForMember(dst => dst.Users, opt => opt
                    .MapFrom(src => src));

            this.CreateMap<ICollection<User>, ICollection<ExportUsersDto>>();
            this.CreateMap<ICollection<ExportUsersDto>, ExportUsersWithProductsDto>();
        }
    }
}
