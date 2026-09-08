using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Helpdesk.Migrations
{
    /// <inheritdoc />
    public partial class TicketRequester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequesterId",
                table: "Tickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Requesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requesters", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Requesters",
                columns: new[] { "Id", "DisplayName", "Login" },
                values: new object[,]
                {
                    { 1, "Wojciech Król", "wojtek" },
                    { 2, "Anna Nowak", "ania" }
                });

            migrationBuilder.Sql(
                """
                UPDATE Tickets
                SET RequesterId = 1
                WHERE RequesterId = 0;
                """);

            migrationBuilder.Sql(
                """
                UPDATE Tickets
                SET Status = 'Open'
                WHERE Status = '';
                """);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RequesterId", "Status" },
                values: new object[] { 1, "Open" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RequesterId", "Status" },
                values: new object[] { 2, "Open" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequesterId",
                table: "Tickets",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Title_RequesterId",
                table: "Tickets",
                columns: new[] { "Title", "RequesterId" },
                unique: true,
                filter: "\"Status\" <> 'Resolved'");

            migrationBuilder.CreateIndex(
                name: "IX_Requesters_Login",
                table: "Requesters",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Requesters_RequesterId",
                table: "Tickets",
                column: "RequesterId",
                principalTable: "Requesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Requesters_RequesterId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Requesters");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_RequesterId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Title_RequesterId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");
        }
    }
}
