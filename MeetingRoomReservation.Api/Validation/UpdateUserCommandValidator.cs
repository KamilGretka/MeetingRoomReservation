using FluentValidation;
using MeetingRoomReservation.Api.Commands;

namespace MeetingRoomReservation.Api.Validation
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            //TODO : Add validation rules for UpdateUserCommand properties, make sure that properties can be null because user is not obligated to change everything at once.
        }
    }
}