using MediatR;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetUsersHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _dbContext.Users
                        .Select(x => new UserDto
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Id = x.Id,
                            Email = x.Email
                        }).ToListAsync(cancellationToken);

            return users;
        }
    }
}
