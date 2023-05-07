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
            
            //Part
            this.CreateMap<ImportPartDto, Part>();
            
            //Car
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<Car, ExportCarDto>();
            this.CreateMap<Car, ExportBmwCarDto>();
            
            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            
            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
