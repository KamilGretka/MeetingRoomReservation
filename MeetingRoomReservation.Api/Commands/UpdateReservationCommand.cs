using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Commands
{
    public class UpdateReservationCommand(Guid Id, UpdateReservationDto UpdateReservationDto) : IRequest<UpdateReservationDto?>
    {
        public Guid Id { get; } = Id;
        public UpdateReservationDto UpdateUserDto { get; } = UpdateReservationDto;
    }
}
