using MediatR;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Interfaces;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            return users.Select(user => new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email
            }).ToList();
        }
    }
}
