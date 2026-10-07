using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetUsersQuery(), cancellationToken);

            if (users == null || !users.Any())
                return NotFound();

            return Ok(users);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(Id), cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new CreateUserCommand(createUserDto), cancellationToken);

            return CreatedAtAction(nameof(GetUserById), user);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(Id));

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var result = await _mediator.Send(new UpdateUserCommand(updateUserDto));

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
