using MeetingRoomReservation.Domain.Models;

namespace MeetingRoomReservation.Domain.Dto
{
    public class CreateReservationDto
    {
        public required Guid Id { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required string? Title { get; set; }
        public required User User { get; set; } = null!;
        public required MeetingRoom Room { get; set; } = null!;
    }
}
