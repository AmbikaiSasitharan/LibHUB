using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class changeNumRatingFromIntToFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "NumRatings",
                table: "BookDescriptions",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumRatings",
                table: "BookDescriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
