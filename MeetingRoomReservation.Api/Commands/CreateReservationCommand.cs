using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Commands
{
    public class CreateReservationCommand(CreateReservationDto createReservationDto) : IRequest<Guid>
    {
        public CreateReservationDto CreateReservationDto { get; } = createReservationDto;
    }
}