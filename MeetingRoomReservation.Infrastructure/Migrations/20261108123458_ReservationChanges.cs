using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoomReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReservationChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reservations",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "HashedPassword");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Reservations",
                newName: "Title");
        }
    }
}
