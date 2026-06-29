using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelService.Application.ApiClientInterface;
using HotelService.Application.Common;
using HotelService.Application.DTOs.Hotel;
using HotelService.Application.DTOs.HotelAndRoom;
using HotelService.Application.Interfaces;
using HotelService.Application.ServiceInterface;
using HotelService.Domain.Entities;

namespace HotelService.Infrastructure.Services
{
    public class HotelManagementService : IHotelService

    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBookingApiClient _bookingApiClient;
        private readonly IRoomApiClient _roomApiClient;
        public HotelManagementService(IHotelRepository repository, IMapper mapper, IRoomApiClient roomApiClient,IBookingApiClient bookingApiClient)
        {
            _repository = repository;
            _mapper = mapper;
            _roomApiClient = roomApiClient;
            _bookingApiClient = bookingApiClient;
        }
        public async Task<Result<HotelDto>> CreateAsync(CreateHotelDto dto)
        {
            if (dto == null) return Result<HotelDto>.Fail("Data Entry is Null");

            var hotel = _mapper.Map<Hotel>(dto);
            await _repository.AddAsync(hotel);
            await _repository.SaveChangesAsync();

            return Result<HotelDto>.Ok(_mapper.Map<HotelDto>(hotel));
        }
        public async Task<Result<List<HotelDto>>> GetAllAsync()
        {
            var hotels = await _repository.GetAllAsync();
            return Result<List<HotelDto>>.Ok(_mapper.Map<List<HotelDto>>(hotels));
        }
        public async Task<Result<HotelDto>> GetByIdAsync(int id)
        {
            var hotel = await _repository.GetByIdAsync(id);

            if (hotel == null) return Result<HotelDto>.Fail("Hotel not Exist");

                return Result<HotelDto>.Ok(_mapper.Map<HotelDto>(hotel));
        }
        public async Task<Result<HotelDto>> UpdateAsync(int id, UpdateHotelDto dto)
        {
            if (dto == null) return Result<HotelDto>.Fail("Data Entry is Null");

            var hotel = await _repository.GetByIdAsync(id);

            if (hotel == null) return Result<HotelDto>.Fail("This Hotel Not Exist");

            var hotelUpdate =_mapper.Map(dto, hotel);

            _repository.Update(hotelUpdate);
            await _repository.SaveChangesAsync();

            return Result<HotelDto>.Ok(_mapper.Map<HotelDto>(hotelUpdate));
        }
        public async Task<Result<HotelDto>> DeleteAsync(int id)
        {
            var hotel = await _repository.GetByIdAsync(id);

            if (hotel == null) return Result<HotelDto>.Fail("This Hotel Not Exist");

            _repository.Delete(hotel);
            await _repository.SaveChangesAsync();

            return Result<HotelDto>.Ok(_mapper.Map<HotelDto>(hotel));
        }
        public async Task<Result<List<ShowListHotelByDetail>>> SearchAvailableHotelRoom(SearchByCityAndDateDto dto)
        {
            if (dto == null)
                return Result<List<ShowListHotelByDetail>>.Fail("Data Entry is Null");

            var hotels = await _repository.GetByCityAsync(dto.City);

            if (hotels == null || !hotels.Any())
                return Result<List<ShowListHotelByDetail>>.Fail("No Hotels Found In This City");

            var hotelIds = hotels.Select(h => h.Id).ToList();


            var rooms = await _roomApiClient.GetRooms(hotelIds);
            if (rooms == null || !rooms.Any())
                return Result<List<ShowListHotelByDetail>>.Fail("No Rooms Found For These Hotels");

            var allRoomIds = rooms.Select(r => r.Id).ToList();


            var availableRoomIds = await _bookingApiClient.CheckAvailableRoom(allRoomIds, dto.CheckInTime, dto.CheckOutTime);
            if (availableRoomIds == null || !availableRoomIds.Any())
                return Result<List<ShowListHotelByDetail>>.Fail("No Available Rooms Found");

            var availableRooms = rooms.Where(r => availableRoomIds.Contains(r.Id)).ToList();

            var result = hotels
                .Select(h => new ShowListHotelByDetail
                {
                    HotelId = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Url = h.Url,
                    Address = h.Address,
                    Rooms = availableRooms.Where(r => r.HotelId == h.Id).ToList()
                })
                .Where(h => h.Rooms.Any())
                .ToList();

            if (!result.Any())
                return Result<List<ShowListHotelByDetail>>.Fail("No Available Hotels Found For This Search");

            return Result<List<ShowListHotelByDetail>>.Ok(result);
        }
    }
}

