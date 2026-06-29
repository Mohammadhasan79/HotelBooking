using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.Common;
using HotelService.Application.DTOs.Hotel;
using HotelService.Application.DTOs.HotelAndRoom;

namespace HotelService.Application.ServiceInterface
{
    public interface IHotelService
    {
        Task<Result<HotelDto>> CreateAsync(CreateHotelDto dto);
        Task<Result<HotelDto>> DeleteAsync(int id);
        Task<Result<HotelDto>> UpdateAsync(int id, UpdateHotelDto dto);
        Task<Result<List<HotelDto>>> GetAllAsync();
        Task<Result<HotelDto>> GetByIdAsync(int id);
        Task<Result<List<ShowListHotelByDetail>>> SearchAvailableHotelRoom(SearchByCityAndDateDto dto);
    }
}
