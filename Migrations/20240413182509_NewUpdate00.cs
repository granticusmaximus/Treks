using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treks.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdate00 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
