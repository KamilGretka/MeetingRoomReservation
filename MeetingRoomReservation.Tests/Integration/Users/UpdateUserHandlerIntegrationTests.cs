using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using MeetingRoomReservation.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeetingRoomReservation.Tests.Integration.Users
{
    public class UpdateUserHandlerIntegrationTests
    {
        [Fact]
        public async Task Handle_UpdatesUserInSqlServerAndPreservesOmittedFields()
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
            var updateUserDto = new UpdateUserDto
            {
                Id = user.Id,
                FirstName = "Augusta",
                LastName = null,
                Email = "augusta.lovelace@example.com",
                Password = null
            };

            await using var dbContext = new AppDbContext(options);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();

            var handler = new UpdateUserHandler(new UserRepository(dbContext), CreateMapper());
            var result = await handler.Handle(new UpdateUserCommand(updateUserDto), CancellationToken.None);

            dbContext.ChangeTracker.Clear();
            var persistedUser = await dbContext.Users
                .AsNoTracking()
                .SingleAsync(existingUser => existingUser.Id == user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("Augusta", result.FirstName);
            Assert.Equal("Lovelace", result.LastName);
            Assert.Equal("augusta.lovelace@example.com", result.Email);
            Assert.Equal("Augusta", persistedUser.FirstName);
            Assert.Equal("Lovelace", persistedUser.LastName);
            Assert.Equal("augusta.lovelace@example.com", persistedUser.Email);
            Assert.Equal("test-password", persistedUser.Password);

            await transaction.RollbackAsync();
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
        }
    }
}
