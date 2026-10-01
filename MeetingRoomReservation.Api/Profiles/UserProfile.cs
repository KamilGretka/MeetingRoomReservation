using AutoMapper;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;

namespace MeetingRoomReservation.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<UpdateUserDto, User>();
        }
    }
}
