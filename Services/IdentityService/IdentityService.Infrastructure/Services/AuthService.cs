using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser != null) return new RegisterResponseDto
            {
                IsSuccess = false,
                Message = "Email already exists"
            };
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return new RegisterResponseDto
            {
                IsSuccess = false,
                Message = string.Join(",",
                    result.Errors.Select(x => x.Description))
            };

            return new RegisterResponseDto
            {
                IsSuccess = true,
                Message = "User created successfully"
            };
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "User Not Found"
            };
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid) return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Invalid credentials"
            };
            
            var token  = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token,
                Message = $"Login Seccessful {user.Email}"
            };
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var secret = _configuration["JwtSettings:Secret"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiry = int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiry),
                signingCredentials : credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
