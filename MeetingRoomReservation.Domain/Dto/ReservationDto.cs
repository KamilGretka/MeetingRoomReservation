namespace MeetingRoomReservation.Domain.Dto
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Title { get; set; }
        public UserDto User { get; set; } = null!;
        public MeetingRoomDto Room { get; set; } = null!;
    }
}
