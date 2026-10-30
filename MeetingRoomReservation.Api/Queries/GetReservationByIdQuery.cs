using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Queries
{
    public class GetReservationByIdQuery(Guid Id) : IRequest<ReservationDto>
    {
        public Guid Id { get; } = Id;
    }
}