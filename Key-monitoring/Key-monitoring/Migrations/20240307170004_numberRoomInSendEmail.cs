using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keymonitoring.Migrations
{
    /// <inheritdoc />
    public partial class numberRoomInSendEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberRoom",
                table: "CodeForEmails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberRoom",
                table: "CodeForEmails");
        }
    }
}
