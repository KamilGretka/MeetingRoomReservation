using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeetingRoomReservation.Tests.Component.Users
{
    public class CreateUserHandlerComponentTests
    {
        [Fact]
        public async Task Handle_CreatesUserAndReturnsItsId()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var dbContext = new AppDbContext(options);
            var mapper = new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
            var handler = new CreateUserHandler(new UserRepository(dbContext), mapper);
            var createUserDto = new CreateUserDto
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "test-password"
            };

            var userId = await handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

            var user = await dbContext.Users.SingleAsync();
            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal(createUserDto.FirstName, user.FirstName);
            Assert.Equal(createUserDto.LastName, user.LastName);
            Assert.Equal(createUserDto.Email, user.Email);
            Assert.Equal(createUserDto.Password, user.Password);
        }
    }
}
