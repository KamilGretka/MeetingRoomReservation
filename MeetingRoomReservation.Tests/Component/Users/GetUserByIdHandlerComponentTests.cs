using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Component.Users
{
    public class GetUserByIdHandlerComponentTests
    {
        [Fact]
        public async Task Handle_ReturnsUserWhenItExists()
        {
            await using var dbContext = CreateDbContext();
            var user = CreateUser();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var handler = new GetUserByIdHandler(new UserRepository(dbContext));

            var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task Handle_ReturnsNullWhenUserDoesNotExist()
        {
            await using var dbContext = CreateDbContext();
            var handler = new GetUserByIdHandler(new UserRepository(dbContext));

            var result = await handler.Handle(new GetUserByIdQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.Null(result);
        }

        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
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
