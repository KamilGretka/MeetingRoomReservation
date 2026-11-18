using Microsoft.AspNetCore.Identity;

namespace MeetingRoomReservation.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public override string Email { get; set; } = string.Empty;
    }
}
