using System.Globalization;
using AutoMapper;
using PetStore.Data.Models;
using PetStore.Web.ViewModels.Categories;
using PetStore.Web.ViewModels.Products;

namespace PetStore.Services.Mapping;

public class PetStoreProfile : Profile
{
    public PetStoreProfile()
    {
        this.CreateMap<CreateCategoryInputModel, Category>();
        this.CreateMap<Category, CategoryListViewModel>();
        this.CreateMap<CategoryListViewModel, Category>()
            .ForMember(dst => dst.Name, opt => opt
                .MapFrom(src => src.Name));
        
        
        //Product
        this.CreateMap<CreateProductInputModel, Product>()
            .ForMember(dst => dst.Price, opt => opt
                .MapFrom(src => decimal.Parse(src.Price, CultureInfo.InvariantCulture)));
        this.CreateMap<Product, ProductViewModel>();
    }
}