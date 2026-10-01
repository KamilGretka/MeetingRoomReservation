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


        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userhandler.GetUsersAsync();

            if (users == null || !users.Any())
                return NotFound();

            return Ok(users);
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
            var user = await _userhandler.CreateUserAsync(createUserDto);

            return CreatedAtAction(nameof(GetUserById), new { user.Id }, user);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            var result = await _userhandler.DeleteUserAsync(Id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var result = await _userhandler.UpdateUserAsync(updateUserDto);

            if (result is null)
                return NotFound();

            return Ok();
        }
    }
}
