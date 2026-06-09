using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoomService.Application.Common;
using RoomService.Application.DTOs;
using RoomService.Application.Interfaces;
using RoomService.Application.RepositoryInterface;
using RoomService.Application.ServiceInterface;
using RoomService.Domain.Entity;

namespace RoomService.Infrastructure.Service
{
    public class RoomServiceManagement : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IHotelApiClient _hotelApiClient;

        public RoomServiceManagement(IRoomRepository roomRepository, IMapper mapper
            , IHotelApiClient hotelApiClient)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _hotelApiClient = hotelApiClient;
        }
        public async Task<Result<RoomDto>> AddAsync(CreateRoomDto Dto)
        {
            var existHotel = await _hotelApiClient.HotelExistsAsync(Dto.HotelId);

            if (!existHotel) return Result<RoomDto>.Fail("Hotel Not Found");

            if (Dto == null) return Result<RoomDto>.Fail("Data Entry is Null");

            var room = _mapper.Map<Room>(Dto);

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            return Result<RoomDto>.Ok(_mapper.Map<RoomDto>(room));
        }
        public async Task<Result<RoomDto>> UpdateAsync(int id, UpdateRoomDto Dto)
        {
            if (Dto == null) return Result<RoomDto>.Fail("Data Entry is Null");

            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return Result<RoomDto>.Fail("Room Not Exist");

            var updateRoom = _mapper.Map(Dto , room);

            _roomRepository.Update(updateRoom);
            await _roomRepository.SaveChangesAsync();
            return Result<RoomDto>.Ok(_mapper.Map<RoomDto>(updateRoom));
        }
        public async Task<Result<RoomDto>> DeleteAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return Result<RoomDto>.Fail("Room Not Exist");

            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync();

            return Result<RoomDto>.Ok(_mapper.Map<RoomDto>(room));
        }
        public async Task<Result<List<RoomDto>>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();

            return Result<List<RoomDto>>.Ok(_mapper.Map<List<RoomDto>>(rooms));
        }
        public async Task<Result<List<RoomDto>>> GetByHotelIdAsync(int hotelId)
        {

            var rooms = await _roomRepository.GetByHotelIdAsync(hotelId);


            return Result<List<RoomDto>>.Ok(_mapper.Map<List<RoomDto>>(rooms));

        }
        public async Task<Result<RoomDto>> GetByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return Result<RoomDto>.Fail("Room Not Exist");

            return Result<RoomDto>.Ok(_mapper.Map<RoomDto>(room));
        }
    }
}
