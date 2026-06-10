using BookingService.Application.DTOs.Booking;
using System.Security.Claims;
using BookingService.Application.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var userId = GetUserId();

                if (userId == null) return Unauthorized();

            var result = await _bookingService.CreateAsync(userId, dto);
            if (!result.Success) return NotFound(result);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookingService.GetAllAsync());
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);

            if (!result.Success) return NotFound(result);

            return Ok(result);
        }
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> MyBookings()
        {
            var userId = GetUserId();

            if (userId == null) return Unauthorized();

            var result = await _bookingService.GetMyBookingsAsync(userId);
            if(!result.Success) return NotFound(result);

            return Ok(result);
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
