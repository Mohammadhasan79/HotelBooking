using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result =await _authService.RegisterAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequestDto Request)
        {
            var result = await _authService.LoginAsync(Request);

            if(!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);

            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
