using AutoMapper;
using FluentValidation;
using MediatR;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly AppDbContext _dbContext;

        public GetUserByIdHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                            .Where(x => x.Id == request.Id)
                            .Select(x => new UserDto
                            {
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Id = x.Id,
                                Email = x.Email
                            })
                            .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
