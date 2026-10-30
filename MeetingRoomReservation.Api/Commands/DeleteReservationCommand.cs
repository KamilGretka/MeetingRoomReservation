using MediatR;

namespace MeetingRoomReservation.Api.Commands
{
    public class DeleteReservationCommand(Guid Id) : IRequest<bool>
    {
        public Guid Id { get; } = Id;
    }
}