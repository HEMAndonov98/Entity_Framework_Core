using AutoMapper;
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
            
            //Category
            this.CreateMap<ImportCategoryDto, Category>();
            
            //CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
