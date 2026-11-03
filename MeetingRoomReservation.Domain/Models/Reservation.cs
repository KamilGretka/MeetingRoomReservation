namespace MeetingRoomReservation.Domain.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public string? Description { get; set; }
        public User User { get; set; } = null!;
        public MeetingRoom Room { get; set; } = null!;
    }
}
