using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawsomeProject.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedReservationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_adopted",
                table: "Animals",
                newName: "Date_adopted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date_adopted",
                table: "Animals",
                newName: "date_adopted");
        }
    }
}
