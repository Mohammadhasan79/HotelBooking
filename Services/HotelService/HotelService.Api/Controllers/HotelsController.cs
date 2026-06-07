using HotelService.Application.DTOs.Hotel;
using HotelService.Application.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHotelDto dto)
        {
            var result = await _hotelService.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hotelService.GetAllAsync();
            if (!result.Success) return BadRequest(result);
                    return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resut = await _hotelService.GetByIdAsync(id);
            if (!resut.Success) return BadRequest(resut);
            return Ok(resut);
        }
    }
}
