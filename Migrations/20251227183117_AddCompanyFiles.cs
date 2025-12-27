using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treks.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "TicketAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_TicketId",
                table: "TicketAttachment",
                newName: "IX_TicketAttachment_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAttachment",
                table: "TicketAttachment",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CompanyFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 260, nullable: false),
                    FileUrl = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyFiles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFiles_CompanyId",
                table: "CompanyFiles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachment_Tickets_TicketId",
                table: "TicketAttachment",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachment_Tickets_TicketId",
                table: "TicketAttachment");

            migrationBuilder.DropTable(
                name: "CompanyFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAttachment",
                table: "TicketAttachment");

            migrationBuilder.RenameTable(
                name: "TicketAttachment",
                newName: "Attachments");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAttachment_TicketId",
                table: "Attachments",
                newName: "IX_Attachments_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
