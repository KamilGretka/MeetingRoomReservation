using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateUserHandler(AppDbContext dbContext, IMapper mapper )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Models.User>(request.CreateUserDto);

            user.Id = Guid.NewGuid();
            user.HashedPassword = request.CreateUserDto.Password; //TODO
                
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
