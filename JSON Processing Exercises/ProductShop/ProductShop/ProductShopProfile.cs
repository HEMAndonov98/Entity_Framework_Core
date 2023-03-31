using AutoMapper;
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
        }
    }
}
