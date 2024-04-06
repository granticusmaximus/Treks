using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treks.Data.Migrations
{
    /// <inheritdoc />
    public partial class TicketCommentsModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Notes",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Notes");
        }
    }
}
