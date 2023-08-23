using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCostAtTimeOfPurchaseToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOwed",
                table: "Borrows");

            migrationBuilder.AddColumn<float>(
                name: "CostAtTimeOfPurchase",
                table: "Books",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostAtTimeOfPurchase",
                table: "Books");

            migrationBuilder.AddColumn<float>(
                name: "AmountOwed",
                table: "Borrows",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
