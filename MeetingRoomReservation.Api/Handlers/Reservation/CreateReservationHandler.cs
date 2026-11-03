using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;

namespace MeetingRoomReservation.Api.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, Guid>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateReservationHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = _mapper.Map<Domain.Models.Reservation>(request.CreateReservationDto);

            reservation.Id = Guid.NewGuid();

            await _dbContext.Reservations.AddAsync(reservation, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return reservation.Id;
        }
    }
}
