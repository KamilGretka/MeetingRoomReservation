using FluentValidation;
using MeetingRoomReservation.Api.Commands;

namespace MeetingRoomReservation.Api.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.CreateUserDto.FirstName)
                .NotEmpty()
                    .WithMessage("First name is required.")
                .MaximumLength(50)
                    .WithMessage("First name cannot exceed 50 characters.")
                .MinimumLength(2)
                    .WithMessage("First name must contain at least 2 characters.");

            RuleFor(x => x.CreateUserDto.LastName)
                .NotEmpty()
                    .WithMessage("Last name is required.")
                .MaximumLength(50)
                    .WithMessage("Last name cannot exceed 50 characters.")
                .MinimumLength(2)
                    .WithMessage("Last name must contain at least 2 characters.");

            RuleFor(x => x.CreateUserDto.Password)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
            .MaximumLength(128)
                .WithMessage("Password cannot exceed 128 characters.")
            .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.CreateUserDto.Email)
                .NotEmpty()
                    .WithMessage("Email is required.")
                .MaximumLength(254)
                    .WithMessage("Email cannot exceed 254 characters.")
                .EmailAddress()
                    .WithMessage("Please insert a valid email address.");
        }
    }
}
