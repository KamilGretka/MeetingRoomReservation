using MeetingRoomReservation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        DbSet<MeetingRoom> MeetingRooms { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Reservation> Reservations { get; set; }
    }
}
