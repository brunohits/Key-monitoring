using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keymonitoring.Migrations
{
    /// <inheritdoc />
    public partial class addKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CabinetNumber = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyModels_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyModels_OwnerId",
                table: "KeyModels",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyModels");
        }
    }
}
