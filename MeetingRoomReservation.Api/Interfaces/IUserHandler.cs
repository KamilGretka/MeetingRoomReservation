using MeetingRoomReservation.Domain.Dto;

namespace MeetingRoomReservation.Api.Interfaces
{
    public interface IUserHandler
    {
        Task<IEnumerable<UserDto>?> GetUsersAsync();

        Task<UserDto?> GetUserByIdAsync(Guid Id);

        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);

        Task<UserDto?> UpdateUserAsync(UpdateUserDto updateUserDto);

        Task<bool> DeleteUserAsync(Guid Id);
    }
}