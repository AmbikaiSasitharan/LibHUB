using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddratingToRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "Rating");
        }
    }
}
