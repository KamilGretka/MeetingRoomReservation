using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Domain.Interfaces;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user, cancellationToken);

            return true;
        }
    }
}
