using System.Net.Http.Json;
using HotelService.Application.ApiClientInterface;
using HotelService.Application.Common;
using HotelService.Application.DTOs.HotelAndRoom;

namespace HotelService.Infrastructure.ExternalService
{
    public class RoomApiClient : IRoomApiClient
    {
        private readonly HttpClient _httpClient;

        public RoomApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RoomDto>> GetRooms(List<int> hotelIds)
        {
            var response = await _httpClient.PostAsJsonAsync("api/rooms/by-hotels", hotelIds);

            if (!response.IsSuccessStatusCode)
                return new List<RoomDto>();

            var result = await response.Content.ReadFromJsonAsync<ApiResult<List<RoomDto>>>();
            return result?.Data ?? new List<RoomDto>();
        }
    }
}