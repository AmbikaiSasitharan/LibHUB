using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibHub.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeManyToManyRelationshipBetweenBDAndAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Writes");

            migrationBuilder.CreateTable(
                name: "AuthorBookDescription",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    BookDescriptionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBookDescription", x => new { x.AuthorsId, x.BookDescriptionsId });
                    table.ForeignKey(
                        name: "FK_AuthorBookDescription_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBookDescription_BookDescriptions_BookDescriptionsId",
                        column: x => x.BookDescriptionsId,
                        principalTable: "BookDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBookDescription_BookDescriptionsId",
                table: "AuthorBookDescription",
                column: "BookDescriptionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBookDescription");

            migrationBuilder.CreateTable(
                name: "Writes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    BookDescriptionId = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writes_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Writes_BookDescriptions_BookDescriptionId",
                        column: x => x.BookDescriptionId,
                        principalTable: "BookDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Writes_AuthorId",
                table: "Writes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Writes_BookDescriptionId",
                table: "Writes",
                column: "BookDescriptionId");
        }
    }
}
