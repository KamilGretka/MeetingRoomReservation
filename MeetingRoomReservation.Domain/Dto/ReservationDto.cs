using MeetingRoomReservation.Domain.Models;

namespace MeetingRoomReservation.Domain.Dto
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Title { get; set; }
        public User User { get; set; } = null!;
        public MeetingRoom Room { get; set; } = null!;
    }
}
