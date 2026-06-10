namespace Shared.Contracts.Events;

public class BookingCreatedEvent
{
    public int BookingId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int RoomId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
}