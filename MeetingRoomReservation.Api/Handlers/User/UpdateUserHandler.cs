using AutoMapper;
using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserDto?>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }   

        public async Task<UpdateUserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                                .FirstOrDefaultAsync(x => x.Id == request.UpdateUserDto.Id, cancellationToken);

            if (user is null)
                return null;

            user.FirstName = request.UpdateUserDto.FirstName;
            user.LastName = request.UpdateUserDto.LastName;
            user.Email = request.UpdateUserDto.Email;
            user.HashedPassword = request.UpdateUserDto.HashedPassword;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateUserDto> (user);
        }
    }
}
