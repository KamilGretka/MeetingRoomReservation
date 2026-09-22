using MeetingRoomReservation.Domain.Enums;

namespace MeetingRoomReservation.Domain.Dto
{
    public class MeetingRoomDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public int Capacity { get; set; }

        public Building Building { get; set; }
    }
}
