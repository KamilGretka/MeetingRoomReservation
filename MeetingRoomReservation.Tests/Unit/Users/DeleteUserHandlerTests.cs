using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Domain.Interfaces;
using MeetingRoomReservation.Domain.Models;
using Moq;

namespace MeetingRoomReservation.Tests.Unit.Users
{
    public class DeleteUserHandlerTests
    {
        [Fact]
        public async Task Handle_DeletesUserAndReturnsTrueWhenUserExists()
        {
            var user = CreateUser();
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            repository
                .Setup(userRepository => userRepository.DeleteAsync(user, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var handler = new DeleteUserHandler(repository.Object);

            var result = await handler.Handle(new DeleteUserCommand(user.Id), CancellationToken.None);

            Assert.True(result);
            repository.Verify(
                userRepository => userRepository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()),
                Times.Once);
            repository.Verify(
                userRepository => userRepository.DeleteAsync(user, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFalseAndDoesNotDeleteWhenUserDoesNotExist()
        {
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            var handler = new DeleteUserHandler(repository.Object);

            var result = await handler.Handle(new DeleteUserCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(result);
            repository.Verify(
                userRepository => userRepository.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Never);
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
