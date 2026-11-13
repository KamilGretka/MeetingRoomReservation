using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Component.Users
{
    public class GetUsersHandlerComponentTests
    {
        [Fact]
        public async Task Handle_ReturnsAllPersistedUsersAsDtos()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var dbContext = new AppDbContext(options);
            var users = new[]
            {
                CreateUser("Ada", "Lovelace", "ada.lovelace@example.com"),
                CreateUser("Grace", "Hopper", "grace.hopper@example.com")
            };
            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
            var handler = new GetUsersHandler(new UserRepository(dbContext));

            var result = (await handler.Handle(new GetUsersQuery(), CancellationToken.None)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, user => user.FirstName == "Ada" && user.LastName == "Lovelace");
            Assert.Contains(result, user => user.FirstName == "Grace" && user.LastName == "Hopper");
            Assert.All(result, user => Assert.False(string.IsNullOrEmpty(user.Email)));
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
