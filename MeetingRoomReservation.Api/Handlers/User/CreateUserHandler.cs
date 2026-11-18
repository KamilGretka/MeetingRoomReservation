using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using Microsoft.AspNetCore.Identity;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Domain.Models.User> _userManager;

        public CreateUserHandler(IMapper mapper, UserManager<Domain.Models.User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Models.User>(request.CreateUserDto);

            user.Id = Guid.NewGuid();
            user.Email = request.CreateUserDto.Email;
            user.UserName = request.CreateUserDto.Email;

            var result = await _userManager.CreateAsync(user, request.CreateUserDto.Password);


                if (!result.Succeeded)
                {
                    var failures = result.Errors.Select(error =>
                        new ValidationFailure(
                            error.Code.Contains("Email", StringComparison.OrdinalIgnoreCase) ||
                            error.Code.Contains("UserName", StringComparison.OrdinalIgnoreCase)
                                ? nameof(request.CreateUserDto.Email)
                                : nameof(request.CreateUserDto.Password),
                            error.Description));

                    throw new ValidationException(failures);
                }

            return user.Id;
        }
    }
}
