using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawsomeProject.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_AnimalId",
                table: "Images",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Animals_AnimalId",
                table: "Images",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Animals_AnimalId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AnimalId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Images");
        }
    }
}
