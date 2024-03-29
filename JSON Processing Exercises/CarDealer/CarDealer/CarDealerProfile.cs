﻿using System.Globalization;
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
            this.CreateMap<ImportCarDto, Car>()
                .ForMember(dst => dst.TraveledDistance, opt => opt
                    .MapFrom(src => src.TraveledDistance));
            this.CreateMap<Car, ExportCarDto>()
                .ForMember(dst => dst.TraveledDistance, opt => opt
                    .MapFrom(src => src.TraveledDistance));


            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<Customer, ExportCustomerDto>()
                .ForMember(dst => dst.BirthDate, opt => opt
                    .MapFrom(src => src.BirthDate.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
            this.CreateMap<Sale, ExportSaleDto>()
                .ForMember(dst => dst.CustomerName, opt => opt
                    .MapFrom(src => src.Customer.Name))
                .ForMember(dst => dst.Price, opt => opt
                    .MapFrom(src => src.Car.PartsCars
                        .Select(pc => pc.Part.Price)
                        .Sum()
                        .ToString()
                    )
                );
        }
    }
}
