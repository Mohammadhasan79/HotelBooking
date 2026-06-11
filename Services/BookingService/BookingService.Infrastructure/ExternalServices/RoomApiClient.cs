using System.Text.Json;
using BookingService.Application.ServiceInterface;

namespace BookingService.Infrastructure.ExternalServices;

public class RoomApiClient : IRoomApiClient
{
    private readonly HttpClient _httpClient;

    public RoomApiClient(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RoomExistAsync(
        int roomId)
    {
        var response = await _httpClient.GetAsync($"api/rooms/{roomId}");

        return response.IsSuccessStatusCode;
    }

    public async Task<decimal> GetRoomPriceAsync(
        int roomId)
    {
        var response = await _httpClient.GetAsync($"api/rooms/{roomId}");

        if (!response.IsSuccessStatusCode) throw new Exception("Room not found");

        var json = await response.Content.ReadAsStringAsync();

        var room = JsonSerializer.Deserialize<RoomResponse>(json,
            new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return room!.PricePerNight;
    }

    private class RoomResponse
    {
        public decimal PricePerNight { get; set; }
    }
}