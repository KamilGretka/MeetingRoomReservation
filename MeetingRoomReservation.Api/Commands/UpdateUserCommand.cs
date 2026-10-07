using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Commands
{
    public class UpdateUserCommand(UpdateUserDto UpdateUserDto) : IRequest<UpdateUserDto?>
    {
        public UpdateUserDto UpdateUserDto { get; } = UpdateUserDto;
    }
}