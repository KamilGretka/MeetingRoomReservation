using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeetingRoomReservation.Tests.Component.Users
{
    public class UpdateUserHandlerComponentTests
    {
        [Fact]
        public async Task Handle_UpdatesProvidedFieldsAndPreservesOmittedFields()
        {
            await using var dbContext = CreateDbContext();
            var user = CreateUser();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var updateUserDto = new UpdateUserDto
            {
                Id = user.Id,
                FirstName = "Augusta",
                LastName = null,
                Email = "augusta.lovelace@example.com",
                Password = null
            };
            var handler = new UpdateUserHandler(new UserRepository(dbContext), CreateMapper());

            var result = await handler.Handle(new UpdateUserCommand(updateUserDto), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("Augusta", result.FirstName);
            Assert.Equal("Lovelace", result.LastName);
            Assert.Equal("augusta.lovelace@example.com", result.Email);

            dbContext.ChangeTracker.Clear();
            var persistedUser = await dbContext.Users.SingleAsync(existingUser => existingUser.Id == user.Id);
            Assert.Equal("Augusta", persistedUser.FirstName);
            Assert.Equal("Lovelace", persistedUser.LastName);
            Assert.Equal("augusta.lovelace@example.com", persistedUser.Email);
            Assert.Equal("test-password", persistedUser.Password);
        }

        [Fact]
        public async Task Handle_ReturnsNullWhenUserDoesNotExist()
        {
            await using var dbContext = CreateDbContext();
            var handler = new UpdateUserHandler(new UserRepository(dbContext), CreateMapper());

            var result = await handler.Handle(
                new UpdateUserCommand(new UpdateUserDto { Id = Guid.NewGuid(), FirstName = "Augusta" }),
                CancellationToken.None);

            Assert.Null(result);
            Assert.Empty(await dbContext.Users.ToListAsync());
        }

        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
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
