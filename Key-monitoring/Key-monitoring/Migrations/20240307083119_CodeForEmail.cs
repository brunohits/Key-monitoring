using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keymonitoring.Migrations
{
    /// <inheritdoc />
    public partial class CodeForEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeForEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    IdFromAdress = table.Column<Guid>(type: "uuid", nullable: false),
                    IdToAdress = table.Column<Guid>(type: "uuid", nullable: false),
                    LifeOfCode = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeForEmails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeForEmails");
        }
    }
}
