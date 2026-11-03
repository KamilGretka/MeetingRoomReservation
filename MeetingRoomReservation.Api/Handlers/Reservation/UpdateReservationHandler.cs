using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class UpdateReservationHandler : IRequestHandler<UpdateReservationCommand, UpdateReservationDto?>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateReservationHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UpdateReservationDto?> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == request.UpdateReservationDto.Id, cancellationToken);

            if (reservation is null)
            {
                return null;
            }

            reservation.StartTime = (DateTime)(request.UpdateReservationDto.StartTime == null ? reservation.StartTime : request.UpdateReservationDto.StartTime);
            reservation.EndTime = (DateTime)(request.UpdateReservationDto.EndTime == null ? reservation.EndTime : request.UpdateReservationDto.EndTime);
            reservation.Description = request.UpdateReservationDto.Description == null ? reservation.Description : request.UpdateReservationDto.Description;
            reservation.Room = request.UpdateReservationDto.Room == null ? reservation.Room : request.UpdateReservationDto.Room;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateReservationDto>(reservation);
        }
    }
}
