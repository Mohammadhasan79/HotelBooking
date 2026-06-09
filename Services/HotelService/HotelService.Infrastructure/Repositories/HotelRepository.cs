using HotelService.Application.Interfaces;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly HotelDbContext _context;
    public HotelRepository(HotelDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
    }

    public void Update(Hotel hotel)
    {
        _context.Hotels.Update(hotel);
    }
    public void Delete(Hotel hotel)
    {
        _context.Hotels.Remove(hotel);
    }

    public async Task<List<Hotel>> GetAllAsync()
    {
        return await _context.Hotels.ToListAsync();
    }
    public async Task<Hotel?> GetByIdAsync(int id)
    {
        return await _context.Hotels.FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }
}