using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supplier
            
            this.CreateMap<ImportSupplierDto, Supplier>()
                .ForMember(dst => dst.Name, opt => opt
                    .MapFrom(src => src.SupplierName))
                .ForMember(dst => dst.IsImporter, opt => opt
                    .MapFrom(src => src.IsImporter));

            this.CreateMap<Supplier, ExportSupplierDto>()
                .ForMember(dst => dst.PartsCount, opt => opt
                    .MapFrom(src => src.Parts.Count));
            
            //Part
            this.CreateMap<ImportPartDto, Part>();
            
            //Car
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<Car, ExportCarDto>();
            
            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<Customer, ExportCustomerDto>();
            
            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
