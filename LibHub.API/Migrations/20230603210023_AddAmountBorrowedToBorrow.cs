using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAmountBorrowedToBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountOwed",
                table: "Borrows",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOwed",
                table: "Borrows");
        }
    }
}
