using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomService.Domain.Entity;

namespace RoomService.Infrastructure.Persistence
{
    public class RoomDbContext : DbContext
    {
        public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options) { }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().Property(a => a.PricePernight)
                 .HasPrecision(18, 2);
        }
    }
}
