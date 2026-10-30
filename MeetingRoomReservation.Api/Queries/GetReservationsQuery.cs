using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Queries
{
    public class GetReservationsQuery : IRequest<IEnumerable<ReservationDto>>
    {
    }
}
