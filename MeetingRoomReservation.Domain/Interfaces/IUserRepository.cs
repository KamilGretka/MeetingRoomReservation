using MeetingRoomReservation.Domain.Models;

namespace MeetingRoomReservation.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
        Task DeleteAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
    }
}
