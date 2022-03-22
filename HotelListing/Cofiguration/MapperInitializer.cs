using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using System;

namespace HotelListing.Cofiguration
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel,CreateHotelDTO>().ReverseMap();
        }
    }
}
