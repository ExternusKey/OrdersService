using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class TableNamesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RejectedOrders",
                table: "RejectedOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmedOrders",
                table: "ConfirmedOrders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "RejectedOrders",
                newName: "rejected_orders");

            migrationBuilder.RenameTable(
                name: "ConfirmedOrders",
                newName: "confirmed_orders");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_product_id",
                table: "orders",
                newName: "IX_orders_product_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rejected_orders",
                table: "rejected_orders",
                column: "rejection_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_confirmed_orders",
                table: "confirmed_orders",
                column: "confirmation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rejected_orders",
                table: "rejected_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_confirmed_orders",
                table: "confirmed_orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "rejected_orders",
                newName: "RejectedOrders");

            migrationBuilder.RenameTable(
                name: "confirmed_orders",
                newName: "ConfirmedOrders");

            migrationBuilder.RenameIndex(
                name: "IX_orders_product_id",
                table: "Orders",
                newName: "IX_Orders_product_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RejectedOrders",
                table: "RejectedOrders",
                column: "rejection_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmedOrders",
                table: "ConfirmedOrders",
                column: "confirmation_id");
        }
    }
}
