using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawsomeProject.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CartItems_CartItemId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CartItemId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "purchasedDate",
                table: "Orders",
                newName: "orderDate");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderItemUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_AspNetUsers_OrderItemUserId",
                        column: x => x.OrderItemUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderItemUserId",
                table: "OrderItems",
                column: "OrderItemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "orderDate",
                table: "Orders",
                newName: "purchasedDate");

            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartItemId",
                table: "Orders",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CartItems_CartItemId",
                table: "Orders",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
