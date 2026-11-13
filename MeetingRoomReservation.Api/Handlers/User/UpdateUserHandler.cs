using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Interfaces;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }   

        public async Task<UpdateUserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UpdateUserDto.Id, cancellationToken);

            if (user is null)
                return null;

            user.FirstName = request.UpdateUserDto.FirstName == null ? user.FirstName : request.UpdateUserDto.FirstName;
            user.LastName = request.UpdateUserDto.LastName == null ? user.LastName : request.UpdateUserDto.LastName;
            user.Email = request.UpdateUserDto.Email == null ? user.Email : request.UpdateUserDto.Email;
            user.Password = request.UpdateUserDto.Password == null ? user.Password : request.UpdateUserDto.Password; //TODO make password hashing

            await _userRepository.UpdateAsync(user, cancellationToken);

            return _mapper.Map<UpdateUserDto> (user);
        }
    }
}
