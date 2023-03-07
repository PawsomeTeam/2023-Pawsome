using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawsomeProject.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAdoptionDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_AspNetUsers_AdopterId",
                table: "Adoptions");

            migrationBuilder.AlterColumn<string>(
                name: "AdopterId",
                table: "Adoptions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_AspNetUsers_AdopterId",
                table: "Adoptions",
                column: "AdopterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_AspNetUsers_AdopterId",
                table: "Adoptions");

            migrationBuilder.AlterColumn<string>(
                name: "AdopterId",
                table: "Adoptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_AspNetUsers_AdopterId",
                table: "Adoptions",
                column: "AdopterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
