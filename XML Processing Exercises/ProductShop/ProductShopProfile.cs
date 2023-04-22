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
            
            //Products
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<Product, ProductsExportDto>()
                .ForMember(dst => dst.BuyerFullName, opt => opt
                    .MapFrom(src =>
                        src.Buyer != null ? 
                            string.Join(" ", src.Buyer.FirstName, src.Buyer.LastName) : string.Empty
                            )
                );
            
            //Category
            this.CreateMap<ImportCategoryDto, Category>();
            
            //CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
