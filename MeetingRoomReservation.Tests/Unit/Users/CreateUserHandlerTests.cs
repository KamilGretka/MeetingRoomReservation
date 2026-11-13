using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Interfaces;
using MeetingRoomReservation.Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace MeetingRoomReservation.Tests.Unit.Users
{
    public class CreateUserHandlerTests
    {
        [Fact]
        public async Task Handle_AddsMappedUserAndReturnsItsId()
        {
            var repository = new Mock<IUserRepository>();
            User? addedUser = null;
            repository
                .Setup(userRepository => userRepository.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((user, _) => addedUser = user)
                .Returns(Task.CompletedTask);
            var handler = new CreateUserHandler(repository.Object, CreateMapper());
            var createUserDto = new CreateUserDto
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "test-password"
            };

            var userId = await handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

            var added = Assert.IsType<User>(addedUser);
            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(userId, added.Id);
            Assert.Equal(createUserDto.FirstName, added.FirstName);
            Assert.Equal(createUserDto.LastName, added.LastName);
            Assert.Equal(createUserDto.Email, added.Email);
            Assert.Equal(createUserDto.Password, added.Password);
            repository.Verify(
                userRepository => userRepository.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
        }
    }
}
