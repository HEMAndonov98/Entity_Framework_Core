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
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<Supplier, ExportSupplierDto>()
                .ForMember(dst => dst.PartsCount, opt => opt
                    .MapFrom(src => src.Parts.Count));
            
            //Part
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<Part, ExportPartDto>();
            
            //Car
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<Car, ExportCarDto>();
            this.CreateMap<Car, ExportBmwCarDto>();
            this.CreateMap<Car, ExportCarsWithPartsDto>()
                .ForMember(dst => dst.Parts, opt => opt
                    .MapFrom(src => src.PartsCars
                        .Select(pc => pc.Part)
                        .OrderByDescending(p => p.Price)));
            
            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            
            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
