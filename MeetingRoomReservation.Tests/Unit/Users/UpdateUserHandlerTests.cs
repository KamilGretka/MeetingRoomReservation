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
    public class UpdateUserHandlerTests
    {
        [Fact]
        public async Task Handle_UpdatesProvidedFieldsAndReturnsUpdatedDto()
        {
            var user = CreateUser();
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            repository
                .Setup(userRepository => userRepository.UpdateAsync(user, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var handler = new UpdateUserHandler(repository.Object, CreateMapper());
            var updateUserDto = new UpdateUserDto
            {
                Id = user.Id,
                FirstName = "Augusta",
                LastName = null,
                Email = "augusta.lovelace@example.com",
                Password = null
            };

            var result = await handler.Handle(new UpdateUserCommand(updateUserDto), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("Augusta", result.FirstName);
            Assert.Equal("Lovelace", result.LastName);
            Assert.Equal("augusta.lovelace@example.com", result.Email);
            Assert.Equal("test-password", user.Password);
            repository.Verify(
                userRepository => userRepository.UpdateAsync(user, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNullAndDoesNotUpdateWhenUserDoesNotExist()
        {
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            var handler = new UpdateUserHandler(repository.Object, CreateMapper());

            var result = await handler.Handle(
                new UpdateUserCommand(new UpdateUserDto { Id = Guid.NewGuid(), FirstName = "Augusta" }),
                CancellationToken.None);

            Assert.Null(result);
            repository.Verify(
                userRepository => userRepository.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
        }

        private static User CreateUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "test-password"
            };
        }
    }
}
