using FluentValidation;
using MeetingRoomReservation.Api.Commands;

namespace MeetingRoomReservation.Api.Validation
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        }
    }
}