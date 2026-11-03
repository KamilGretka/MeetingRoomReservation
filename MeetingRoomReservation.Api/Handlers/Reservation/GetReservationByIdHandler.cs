using MediatR;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto?>
    {
        private readonly AppDbContext _dbContext;

        public GetReservationByIdHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReservationDto?> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Reservations
                .Where(x => x.Id == request.Id)
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Title = x.Description,
                    Room = new Domain.Models.MeetingRoom
                    {
                        Id = x.Room.Id,
                        Name = x.Room.Name,
                        Building = x.Room.Building,
                        Capacity = x.Room.Capacity,
                        Description = x.Room.Description
                    },
                    User = new Domain.Models.User
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        Password = x.User.Password
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (reservation == null)
            {
                return null;
            }

            return reservation;
        }
    }
}
