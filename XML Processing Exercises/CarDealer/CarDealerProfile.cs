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
            this.CreateMap<Customer, ExportCustomerDto>()
                .ForMember(dst => dst.BoughtCars, opt => opt
                    .MapFrom(src => src.Sales.Count))
                .ForMember(dst => dst.SpentMoney, opt => opt
                    .MapFrom(src => CalculateSpentMoney(src)));
            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
        
        //Custom resolver would not work so I had coded the mapping function
        private static decimal CalculateSpentMoney(Customer customer)
        {
            decimal totalSpent = 0;

            foreach (var sale in customer.Sales)
            {
                decimal discount = 0;

                if (customer.IsYoungDriver)
                {
                    discount = (decimal)0.05;
                }

                var totalPartsCost = sale.Car.PartsCars
                    .Sum(pc => pc.Part.Price);

                totalSpent += totalPartsCost - (totalPartsCost * discount);
            }

            return decimal.Round(totalSpent, 2, MidpointRounding.ToZero);
        }
    }
}
