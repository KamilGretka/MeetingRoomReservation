using MediatR;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class CheckRoomAvailabilityHandler : IRequestHandler<CheckRoomAvailabilityQuery, bool>
    {
        private readonly AppDbContext _dbContext;

        public CheckRoomAvailabilityHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckRoomAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Reservations.AnyAsync(x => x.StartTime < request.EndTime && x.EndTime > request.StartTime, cancellationToken);

            return result;
        }
    }
}
