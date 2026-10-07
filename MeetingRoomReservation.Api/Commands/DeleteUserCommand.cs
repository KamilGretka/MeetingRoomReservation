using MediatR;

namespace MeetingRoomReservation.Api.Commands
{
    public class DeleteUserCommand(Guid Id) : IRequest<bool>
    {        
        public Guid Id { get; } = Id;
    }
}
