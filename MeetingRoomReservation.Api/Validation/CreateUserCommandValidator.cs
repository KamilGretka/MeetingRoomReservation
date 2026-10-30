using FluentValidation;
using MeetingRoomReservation.Api.Commands;

namespace MeetingRoomReservation.Api.Validators
{
    public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.CreateUserDto.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.");

            RuleFor(x => x.CreateUserDto.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");

            RuleFor(x => x.CreateUserDto.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            RuleFor(x => x.CreateUserDto.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please insert a valid email address.");

            //TODO make better validations
        }
    }
}
