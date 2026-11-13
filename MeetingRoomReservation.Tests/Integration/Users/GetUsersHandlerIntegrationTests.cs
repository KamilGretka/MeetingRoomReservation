using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using MeetingRoomReservation.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Integration.Users
{
    public class GetUsersHandlerIntegrationTests
    {
        [Fact]
        public async Task Handle_ReturnsUsersPersistedInSqlServer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(TestConfiguration.GetConnectionString("DefaultConnection"))
                .Options;
            var users = new[]
            {
                CreateUser("Ada", "Lovelace", "ada.lovelace@example.com"),
                CreateUser("Grace", "Hopper", "grace.hopper@example.com")
            };

            await using var dbContext = new AppDbContext(options);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();

            var handler = new GetUsersHandler(new UserRepository(dbContext));
            var result = (await handler.Handle(new GetUsersQuery(), CancellationToken.None)).ToList();

            foreach (var expectedUser in users)
            {
                var actualUser = Assert.Single(result.Where(user => user.Id == expectedUser.Id));
                Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
                Assert.Equal(expectedUser.LastName, actualUser.LastName);
                Assert.Equal(expectedUser.Email, actualUser.Email);
            }

            await transaction.RollbackAsync();
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
