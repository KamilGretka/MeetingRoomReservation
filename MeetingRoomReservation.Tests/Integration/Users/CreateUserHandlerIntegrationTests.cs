using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Infrastructure.Repositories;
using MeetingRoomReservation.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeetingRoomReservation.Tests.Integration.Users
{
    public class CreateUserHandlerIntegrationTests
    {
        [Fact]
        public async Task Handle_CreatesUserThatCanBeReadFromSqlServer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(TestConfiguration.GetConnectionString("DefaultConnection"))
                .Options;
            var mapper = new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
            var createUserDto = new CreateUserDto
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "test-password"
            };

            await using var dbContext = new AppDbContext(options);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var handler = new CreateUserHandler(new UserRepository(dbContext), mapper);
            var userId = await handler.Handle(
                new CreateUserCommand(createUserDto),
                CancellationToken.None);

            dbContext.ChangeTracker.Clear();
            var user = await dbContext.Users
                .AsNoTracking()
                .SingleAsync(user => user.Id == userId);

            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(createUserDto.FirstName, user.FirstName);
            Assert.Equal(createUserDto.LastName, user.LastName);
            Assert.Equal(createUserDto.Email, user.Email);
            Assert.Equal(createUserDto.Password, user.Password);

            await transaction.RollbackAsync();
        }
    }
}
