using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Interfaces
{
    public interface IUserHandler
    {
       Task<UserDto?> GetUserByIdAsync(Guid Id); 

       Task<UserDto> AddUserAsync(CreateUserDto createUserDto);
    }
}