using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawsomeProject.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Images",
                newName: "URL");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Images",
                newName: "Data");
        }
    }
}
