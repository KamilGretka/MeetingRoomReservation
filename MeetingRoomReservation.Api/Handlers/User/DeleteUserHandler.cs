using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Handlers.User
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteUserHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null)
            {
                return false;
            }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return false;
            }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
