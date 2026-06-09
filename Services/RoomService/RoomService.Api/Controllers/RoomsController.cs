using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomService.Application.DTOs;
using RoomService.Application.ServiceInterface;

namespace RoomService.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var resut = await _roomService.GetByIdAsync(id);
            if (!resut.Success) return BadRequest(resut);
            return Ok(resut);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var resut = await _roomService.GetByHotelIdAsync(hotelId);
            if (!resut.Success) return BadRequest(resut);
            return Ok(resut);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            var result = await _roomService.AddAsync(dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateRoomDto dto)
        {
            var result = await _roomService.UpdateAsync(id, dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteAsync(id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}

