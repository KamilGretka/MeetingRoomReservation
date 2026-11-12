using FluentValidation;
using MeetingRoomReservation.Api.Commands;

namespace MeetingRoomReservation.Api.Validation.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UpdateUserDto.FirstName)
                .NotEmpty()
                    .WithMessage("First name is required.")
                .MaximumLength(50)
                    .WithMessage("First name cannot exceed 50 characters.")
                .MinimumLength(2)
                    .WithMessage("First name must contain at least 2 characters.")
                .When(x => x.UpdateUserDto.FirstName != null);

            RuleFor(x => x.UpdateUserDto.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .When(x => x.UpdateUserDto.LastName != null);

            RuleFor(x => x.UpdateUserDto.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100)
                .When(x => x.UpdateUserDto.Email != null);

            RuleFor(x => x.UpdateUserDto.Password)
                .MinimumLength(8)
                .MaximumLength(100)
                .When(x => x.UpdateUserDto.Password != null);
        }
    }
}