using MeetingRoomReservation.Domain.Enums;

namespace MeetingRoomReservation.Domain.Models
{
    public class MeetingRoom
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        
        public required string Description { get; set; }

        public int Capacity { get; set; }

        public Building Building { get; set; }
    }
}
