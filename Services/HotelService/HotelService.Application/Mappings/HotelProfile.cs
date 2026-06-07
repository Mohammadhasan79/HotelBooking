using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelService.Application.DTOs.Hotel;
using HotelService.Domain.Entities;

namespace HotelService.Application.Mappings
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<CreateHotelDto, Hotel>();
            CreateMap<Hotel, HotelDto>();  
        }
    }
}
