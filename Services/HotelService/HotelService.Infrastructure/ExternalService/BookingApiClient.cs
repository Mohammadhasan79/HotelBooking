using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HotelService.Application.ApiClientInterface;
using HotelService.Application.Common;
using HotelService.Application.DTOs.HotelAndRoom;

namespace HotelService.Infrastructure.ExternalService
{
    public class BookingApiClient : IBookingApiClient
    {
        private readonly HttpClient _httpClient;

        public BookingApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> CheckAvailableRoom(List<int> roomIds, DateTime checkInTime, DateTime checkOutTime)
        {
            if (roomIds == null || !roomIds.Any())
                return new List<int>();

            var requestDto = new CheckAvailableRequestDto
            {
                RoomId = roomIds,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime
            };

            var response = await _httpClient.PostAsJsonAsync("api/bookings/CheckAvailableRoom", requestDto);

            if (!response.IsSuccessStatusCode)
                return new List<int>();

            var result = await response.Content.ReadFromJsonAsync<ApiResult<List<int>>>();
            return result?.Data ?? new List<int>();
        }
    }
}