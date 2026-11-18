using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingRoomReservation.Tests.Common
{
    public static class IdentityTestServices
    {
        public static ServiceProvider CreateProvider(AppDbContext dbContext)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(dbContext);
            services
                .AddIdentityCore<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            return services.BuildServiceProvider();
        }
    }
}
