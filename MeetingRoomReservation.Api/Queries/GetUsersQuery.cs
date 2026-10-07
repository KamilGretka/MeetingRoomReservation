using MediatR;
using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {      
    }
}