using AutoMapper;
using Newtonsoft.Json.Linq;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<JToken, ImportUserDto>();
        }
    }
}
