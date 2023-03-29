using FastFood.Web.ViewModels.Employees;
using FastFood.Web.ViewModels.Items;

namespace FastFood.Services.Mapping
{
    using AutoMapper;
    
    using Models;
    using Web.ViewModels.Positions;
    
    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));
        
            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));
            
            //Employees
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(dst => 
                    dst.PositionId, opt => opt
                            .MapFrom(src => src.Id));

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(dst => dst.Position, 
                    opt => opt
                        .MapFrom(src => src.Position.Name));
            
            this.CreateMap<RegisterEmployeeInputModel, Employee>();
            
            //Items
            
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(dst => dst.CategoryId, opt => opt
                    .MapFrom(src => src.Id));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(dst => dst.Category, opt => opt
                    .MapFrom(src => src.Category.Name));
        }
    }
}