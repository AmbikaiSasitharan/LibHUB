using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToBookDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BookDescriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BookDescriptions");
        }
    }
}
