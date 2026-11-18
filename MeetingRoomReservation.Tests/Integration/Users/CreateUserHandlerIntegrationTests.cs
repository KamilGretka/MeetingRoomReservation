using AutoMapper;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using MeetingRoomReservation.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                Password = "Test-password1"
            };

            await using var dbContext = new AppDbContext(options);
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            using var services = IdentityTestServices.CreateProvider(dbContext);
            var userManager = services.GetRequiredService<UserManager<User>>();

            var handler = new CreateUserHandler(mapper, userManager);
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
            Assert.NotNull(user.PasswordHash);
            Assert.NotEqual(createUserDto.Password, user.PasswordHash);

            await transaction.RollbackAsync();
        }
    }
}
