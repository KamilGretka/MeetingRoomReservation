using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Domain.Interfaces;
using UserModel = MeetingRoomReservation.Domain.Models.User;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserModel>(request.CreateUserDto);

            user.Id = Guid.NewGuid();
            user.Password = request.CreateUserDto.Password; //TODO make password hashing
                
            await _userRepository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}
