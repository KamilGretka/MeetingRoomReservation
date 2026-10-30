using MediatR;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class GetReservationsHandler : IRequestHandler<GetReservationsQuery, IEnumerable<ReservationDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetReservationsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _dbContext.Reservations
            .Select(x => new ReservationDto
            {
               Id = x.Id,
               StartTime = x.StartTime,
                EndTime = x.EndTime,
                Title = x.Title,
                User = new Domain.Models.User
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    Password = x.User.Password
                },
                Room = new Domain.Models.MeetingRoom
                {
                    Id = x.Room.Id,
                    Name = x.Room.Name,
                    Building = x.Room.Building,
                    Capacity = x.Room.Capacity,
                    Description = x.Room.Description
                }

            }).ToListAsync(cancellationToken);

            return reservations;
        }
    }
}