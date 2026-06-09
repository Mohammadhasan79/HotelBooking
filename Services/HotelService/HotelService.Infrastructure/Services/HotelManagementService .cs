using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelService.Application.Common;
using HotelService.Application.DTOs.Hotel;
using HotelService.Application.Interfaces;
using HotelService.Application.ServiceInterface;
using HotelService.Domain.Entities;

namespace HotelService.Infrastructure.Services
{
    public class HotelManagementService : IHotelService

    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        public HotelManagementService(IHotelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
    }
}

