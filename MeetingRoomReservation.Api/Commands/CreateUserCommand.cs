using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Commands
{
    public record CreateUserCommand(CreateUserDto CreateUserDto) : IRequest<Guid>
    {
        public CreateUserDto CreateUserDto { get; } = CreateUserDto;
    }
}
