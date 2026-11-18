using AutoMapper;
using FluentValidation;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Domain.Dto;
using MeetingRoomReservation.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace MeetingRoomReservation.Tests.Unit.Users
{
    public class CreateUserHandlerTests
    {
        [Fact]
        public async Task Handle_CreatesIdentityUserAndReturnsItsId()
        {
            var userManager = CreateUserManager();
            User? createdUser = null;
            var createUserDto = CreateValidDto();
            userManager
                .Setup(manager => manager.CreateAsync(It.IsAny<User>(), createUserDto.Password))
                .Callback<User, string>((user, _) => createdUser = user)
                .ReturnsAsync(IdentityResult.Success);
            var handler = new CreateUserHandler(CreateMapper(), userManager.Object);

            var userId = await handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

            var user = Assert.IsType<User>(createdUser);
            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal(createUserDto.FirstName, user.FirstName);
            Assert.Equal(createUserDto.LastName, user.LastName);
            Assert.Equal(createUserDto.Email, user.Email);
            Assert.Equal(createUserDto.Email, user.UserName);
            Assert.Null(user.PasswordHash);
            userManager.Verify(manager => manager.CreateAsync(user, createUserDto.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsValidationExceptionWhenIdentityRejectsUser()
        {
            var userManager = CreateUserManager();
            var createUserDto = CreateValidDto();
            userManager
                .Setup(manager => manager.CreateAsync(It.IsAny<User>(), createUserDto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Password is too short."
                }));
            var handler = new CreateUserHandler(CreateMapper(), userManager.Object);

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None));

            Assert.Contains(exception.Errors, error =>
                error.PropertyName == nameof(MeetingRoomReservation.Domain.Dto.CreateUserDto.Password) &&
                error.ErrorMessage == "Password is too short.");
        }

        private static Mock<UserManager<User>> CreateUserManager()
        {
            var userStore = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                userStore.Object,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<User>(),
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>());
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(
                configuration => configuration.AddProfile<UserProfile>(),
                NullLoggerFactory.Instance)
                .CreateMapper();
        }

        private static CreateUserDto CreateValidDto()
        {
            return new CreateUserDto
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                Password = "Test-password1"
            };
        }
    }
}
