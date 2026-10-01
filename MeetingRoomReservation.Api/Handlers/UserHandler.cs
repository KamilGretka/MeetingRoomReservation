using AutoMapper;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Interfaces;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid Id)
        {
            var user = await _dbContext.Users
                            .Where(x => x.Id == Id)
                            .Select(x => new UserDto
                            {
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Id = x.Id,
                                Email = x.Email
                            })
                            .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email
            };
        }

        public Task<IEnumerable<UserDto>?> GetUsersAsync()
        {
            var users = _dbContext.Users
                .Select(x => new UserDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Id = x.Id,
                    Email = x.Email
                })
                .ToList();

            return users.AsEnumerable<UserDto?>() is null ? Task.FromResult<IEnumerable<UserDto>?>(null) : Task.FromResult<IEnumerable<UserDto>?>(users);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);

            user.Id = Guid.NewGuid();
            user.HashedPassword = "_passwordHasher.Hash(password)";

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

            if (user is null)
                return null;

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if(user == null)
            {
                return false;
            }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
