using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Interfaces;
using MeetingRoomReservation.Domain.Models;
using Moq;

namespace MeetingRoomReservation.Tests.Unit.Users
{
    public class GetUserByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsUserDtoWhenUserExists()
        {
            var user = CreateUser();
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            var handler = new GetUserByIdHandler(repository.Object);

            var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Email, result.Email);
            repository.Verify(
                userRepository => userRepository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNullWhenUserDoesNotExist()
        {
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            var handler = new GetUserByIdHandler(repository.Object);

            var result = await handler.Handle(new GetUserByIdQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.Null(result);
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
