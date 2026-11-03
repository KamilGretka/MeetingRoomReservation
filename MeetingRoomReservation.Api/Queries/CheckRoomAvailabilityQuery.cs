using MediatR;

namespace MeetingRoomReservation.Api.Queries
{
    public class CheckRoomAvailabilityQuery(DateTime StartTime, DateTime EndTime) : IRequest<bool>
    {
        public DateTime StartTime { get; } = StartTime;
        public DateTime EndTime { get; } = EndTime;
    }
}