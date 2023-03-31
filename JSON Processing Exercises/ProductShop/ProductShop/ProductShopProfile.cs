using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //User Profile
            this.CreateMap<ImportUserDto, User>();
            
            //Product Profile
            this.CreateMap<ImportProductDto, Product>();
            
            //Category Profile
            this.CreateMap<ImportCategoryDto, Category>();
            
            //CategoryProduct Profile
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
