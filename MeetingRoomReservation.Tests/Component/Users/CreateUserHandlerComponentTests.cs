using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

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
            using var services = IdentityTestServices.CreateProvider(dbContext);
            var userManager = services.GetRequiredService<UserManager<User>>();
            var handler = new CreateUserHandler(mapper, userManager);
            var createUserDto = new CreateUserDto
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "Test-password1"
            };

            var userId = await handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

            var user = await dbContext.Users.SingleAsync();
            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal(createUserDto.FirstName, user.FirstName);
            Assert.Equal(createUserDto.LastName, user.LastName);
            Assert.Equal(createUserDto.Email, user.Email);
            Assert.NotNull(user.PasswordHash);
            Assert.NotEqual(createUserDto.Password, user.PasswordHash);
        }
    }
}
