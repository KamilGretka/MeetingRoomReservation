using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteReservationHandler(AppDbContext dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (reservation == null)
            {
                return false;
            }

            _dbContext.Remove(reservation);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
