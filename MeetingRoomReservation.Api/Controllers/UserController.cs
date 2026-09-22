using MeetingRoomReservation.Api.Interfaces;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserHandler _userhandler;

        public UserController(IUserHandler userHandler)
        {
            _userhandler = userHandler;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
           var user = await _userhandler.GetUserByIdAsync(Id);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(CreateUserDto createUserDto)
        {
            var user = await _userhandler.AddUserAsync(createUserDto);

            //TODO add some validation if that user already exists

            return CreatedAtAction(nameof(GetUserById), new { user.Id }, user);
        }
    }
}
