using RoomService.Application.Interfaces;

namespace RoomService.Infrastructure.ExternalServices;

public class HotelApiClient : IHotelApiClient
{
    private readonly HttpClient _httpClient;

    public HotelApiClient(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> HotelExistsAsync(
        int hotelId)
    {
        var response =
            await _httpClient.GetAsync(
                $"api/hotels/{hotelId}");

        return response.IsSuccessStatusCode;
    }
}