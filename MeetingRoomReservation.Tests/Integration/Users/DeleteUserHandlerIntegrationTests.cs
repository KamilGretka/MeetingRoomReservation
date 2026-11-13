using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using MeetingRoomReservation.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Integration.Users
{
    public class DeleteUserHandlerIntegrationTests
    {
        [Fact]
        public async Task Handle_DeletesUserFromSqlServer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(TestConfiguration.GetConnectionString("DefaultConnection"))
                .Options;
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "test-password"
            };

            await using var dbContext = new AppDbContext(options);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var handler = new DeleteUserHandler(new UserRepository(dbContext));
            var result = await handler.Handle(new DeleteUserCommand(user.Id), CancellationToken.None);

            dbContext.ChangeTracker.Clear();
            var deletedUser = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(existingUser => existingUser.Id == user.Id);

            Assert.True(result);
            Assert.Null(deletedUser);

            await transaction.RollbackAsync();
        }
    }
}
