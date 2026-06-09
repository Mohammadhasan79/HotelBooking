using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoomService.Application.DTOs;
using RoomService.Domain.Entity;

namespace RoomService.Application.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
            CreateMap<Room, RoomDto>();
        }
    }
}
