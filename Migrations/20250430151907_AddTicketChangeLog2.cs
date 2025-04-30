using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treks.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketChangeLog2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketChangeLogs_AspNetUsers_ChangedByUserId",
                table: "TicketChangeLogs");

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "TicketChangeLogs");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "TicketChangeLogs");

            migrationBuilder.RenameColumn(
                name: "FieldChanged",
                table: "TicketChangeLogs",
                newName: "ChangeDescription");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TicketChangeLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketChangeLogs_AspNetUsers_ChangedByUserId",
                table: "TicketChangeLogs",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketChangeLogs_AspNetUsers_ChangedByUserId",
                table: "TicketChangeLogs");

            migrationBuilder.RenameColumn(
                name: "ChangeDescription",
                table: "TicketChangeLogs",
                newName: "FieldChanged");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TicketChangeLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "TicketChangeLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                table: "TicketChangeLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketChangeLogs_AspNetUsers_ChangedByUserId",
                table: "TicketChangeLogs",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
