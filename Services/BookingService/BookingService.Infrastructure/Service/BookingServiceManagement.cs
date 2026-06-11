using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookingService.Application.Common;
using BookingService.Application.DTOs.Booking;
using BookingService.Application.RepositoryInterface;
using BookingService.Application.ServiceInterface;
using BookingService.Domain.Entities;
using BookingService.Domain.Enums;
using Shared.Contracts.Events;

namespace BookingService.Infrastructure.Service
{
    public class BookingServiceManagement : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomApiClient _roomApiClient;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _publisher;

        public BookingServiceManagement(
            IBookingRepository repository,
            IRoomApiClient roomApiClient,
            IMapper mapper,
            IMessagePublisher publisher)
        {
            _repository = repository;
            _roomApiClient = roomApiClient;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<Result<List<BookingDto>>> GetAllAsync()
        {
            var bookings = await _repository.GetAllAsync();
            return Result<List<BookingDto>>.Ok(_mapper.Map<List<BookingDto>>(bookings));
        }
        public async Task<Result<BookingDto?>> GetByIdAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null) return Result<BookingDto?>.Fail("Booking Not Exist");

            return Result<BookingDto?>.Ok(_mapper?.Map<BookingDto>(booking));
        }
        public async Task<Result<BookingDto>> CreateAsync(string userId, CreateBookingDto dto)
        {
            var roomExists = await _roomApiClient.RoomExistAsync(dto.RoomId);

            if (!roomExists) return Result<BookingDto>.Fail("Room not found");

            var roomBookings = await _repository.GetByRoomIdAsync(dto.RoomId);

            foreach (var booking in roomBookings)
            {
                if (booking.Status == BookingStatus.Cancelled)
                    continue;

                if (HasDateConflict(booking,dto))
                {
                    return Result<BookingDto>.Fail("Room already booked for selected dates");
                }
            }

            var roomPrice = await _roomApiClient.GetRoomPriceAsync(dto.RoomId);

            var totalNights = (dto.CheckOutDate - dto.CheckInDate).Days;

            var totalPrice = totalNights * roomPrice;

            var newBooking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,

                CheckOutDate = dto.CheckOutDate,

                TotalPrice = totalPrice,

                Status = BookingStatus.Pending,

                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(newBooking);

            await _repository.SaveChangesAsync();

            var bookingCreatedEvent = new BookingCreatedEvent
                {
                    BookingId = newBooking.Id,

                    UserId = newBooking.UserId,

                    RoomId = newBooking.RoomId,

                    TotalPrice = newBooking.TotalPrice,

                    CreatedAt = newBooking.CreatedAt
                };

            await _publisher.Publish(bookingCreatedEvent);

            return Result<BookingDto>.Ok(_mapper.Map<BookingDto>(newBooking));

        }

        public async Task<Result<List<BookingDto>>> GetMyBookingsAsync(string userId)
        {
            var bookings = await _repository.GetByUserIdAsync(userId);

            return Result<List<BookingDto>>.Ok(_mapper.Map<List<BookingDto>>(bookings));
        }


        public async Task<Result> CancelAsync(int bookingId, string userId)
        {
            var booking = await _repository.GetByIdAsync(bookingId);

            if (booking == null) return Result.Fail("Reserve Not Exist");

            if (booking.UserId != userId) return Result.Fail("Access Denied");

            booking.Status = BookingStatus.Cancelled;

            await _repository.SaveChangesAsync();
            return Result.Ok("Reserve Cancelled");
        }


        private bool HasDateConflict(Booking existingBooking, CreateBookingDto dto)
        {
            return dto.CheckInDate < existingBooking.CheckOutDate && dto.CheckOutDate > existingBooking.CheckInDate;
        }
        
    }
}
