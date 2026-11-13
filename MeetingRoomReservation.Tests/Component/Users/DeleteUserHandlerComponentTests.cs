using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Component.Users
{
    public class DeleteUserHandlerComponentTests
    {
        [Fact]
        public async Task Handle_DeletesExistingUserAndReturnsTrue()
        {
            await using var dbContext = CreateDbContext();
            var user = CreateUser();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var handler = new DeleteUserHandler(new UserRepository(dbContext));

            var result = await handler.Handle(new DeleteUserCommand(user.Id), CancellationToken.None);

            Assert.True(result);
            Assert.Empty(await dbContext.Users.ToListAsync());
        }

        [Fact]
        public async Task Handle_ReturnsFalseWhenUserDoesNotExist()
        {
            await using var dbContext = CreateDbContext();
            var handler = new DeleteUserHandler(new UserRepository(dbContext));

            var result = await handler.Handle(new DeleteUserCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(result);
            Assert.Empty(await dbContext.Users.ToListAsync());
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
