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
            
            //Users
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<User, ExportSellerDto>()
                .ForMember(dst => dst.SoldProducts, opt => opt
                    .MapFrom(src => src.ProductsSold.ToArray()));
            
            //Products
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<Product, ProductsExportDto>()
                .ForMember(dst => dst.BuyerFullName, opt => opt
                    .MapFrom(src =>
                        src.Buyer != null ? 
                            string.Join(" ", src.Buyer.FirstName, src.Buyer.LastName) : string.Empty
                            )
                );
            this.CreateMap<Product, ExportSellerProductDto>();
            
            //Category
            this.CreateMap<ImportCategoryDto, Category>();
            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(dst => dst.TotalRevenue, opt => opt
                    .MapFrom(src => src.CategoryProducts
                        .Select(cp => cp.Product.Price).Sum()))
                .ForMember(dst => dst.ProductCount, opt => opt
                    .MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dst => dst.AveragePrice, opt => opt
                    .MapFrom(src =>
                        (src.CategoryProducts.Select(cp => cp.Product.Price).Sum() / src.CategoryProducts.Count)));
            //CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
