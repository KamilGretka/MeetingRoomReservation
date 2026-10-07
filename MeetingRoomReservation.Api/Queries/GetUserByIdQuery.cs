using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Queries
{
    public class GetUserByIdQuery(Guid Id) : IRequest<UserDto?>
    {
        public Guid Id { get; } = Id;
    }
}
