using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using MeetingRoomReservation.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Tests.Integration.Users
{
    public class GetUserByIdHandlerIntegrationTests
    {
        [Fact]
        public async Task Handle_ReturnsUserFromSqlServer()
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
            dbContext.ChangeTracker.Clear();

            var handler = new GetUserByIdHandler(new UserRepository(dbContext));
            var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Email, result.Email);

            await transaction.RollbackAsync();
        }
    }
}
