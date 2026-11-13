using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Interfaces;
using MeetingRoomReservation.Domain.Models;
using Moq;

namespace MeetingRoomReservation.Tests.Unit.Users
{
    public class GetUsersHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsUserDtosFromRepository()
        {
            IReadOnlyList<User> users =
            [
                CreateUser("Ada", "Lovelace", "ada.lovelace@example.com"),
                CreateUser("Grace", "Hopper", "grace.hopper@example.com")
            ];
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);
            var handler = new GetUsersHandler(repository.Object);

            var result = (await handler.Handle(new GetUsersQuery(), CancellationToken.None)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(users[0].Id, result[0].Id);
            Assert.Equal("Ada", result[0].FirstName);
            Assert.Equal("Lovelace", result[0].LastName);
            Assert.Equal("ada.lovelace@example.com", result[0].Email);
            Assert.Equal(users[1].Id, result[1].Id);
            Assert.Equal("Grace", result[1].FirstName);
            Assert.Equal("Hopper", result[1].LastName);
            Assert.Equal("grace.hopper@example.com", result[1].Email);
            repository.Verify(
                userRepository => userRepository.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyListWhenRepositoryHasNoUsers()
        {
            var repository = new Mock<IUserRepository>();
            repository
                .Setup(userRepository => userRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<User>());
            var handler = new GetUsersHandler(repository.Object);

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.Empty(result);
        }

        private static User CreateUser(string firstName, string lastName, string email)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = "test-password"
            };
        }
    }
}
