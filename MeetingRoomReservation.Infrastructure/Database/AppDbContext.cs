using MeetingRoomReservation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }

        public DbSet<MeetingRoom> MeetingRooms { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
