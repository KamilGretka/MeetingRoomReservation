using MeetingRoomReservation.Domain.Models;

namespace MeetingRoomReservation.Domain.Dto
{
    public class UpdateReservationDto
    {
        public Guid Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
        public User? User { get; set; } = null!;
        public MeetingRoom? Room { get; set; } = null!;
    }
}
