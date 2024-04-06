using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treks.Data.Migrations
{
    /// <inheritdoc />
    public partial class TicketCommentsModelUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Notes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Notes");
        }
    }
}
