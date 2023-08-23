using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MName",
                table: "Users");
        }
    }
}
