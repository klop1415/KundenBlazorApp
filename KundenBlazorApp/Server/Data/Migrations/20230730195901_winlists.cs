using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class winlists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kunden_WinKundeListe_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropColumn(
                name: "WinKundeListId",
                table: "Kunden");

            migrationBuilder.CreateTable(
                name: "KundeWinKundeList",
                columns: table => new
                {
                    ListId = table.Column<int>(type: "int", nullable: false),
                    WinKundeListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KundeWinKundeList", x => new { x.ListId, x.WinKundeListsId });
                    table.ForeignKey(
                        name: "FK_KundeWinKundeList_Kunden_ListId",
                        column: x => x.ListId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KundeWinKundeList_WinKundeListe_WinKundeListsId",
                        column: x => x.WinKundeListsId,
                        principalTable: "WinKundeListe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KundeWinKundeList_WinKundeListsId",
                table: "KundeWinKundeList",
                column: "WinKundeListsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KundeWinKundeList");

            migrationBuilder.AddColumn<int>(
                name: "WinKundeListId",
                table: "Kunden",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden",
                column: "WinKundeListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kunden_WinKundeListe_WinKundeListId",
                table: "Kunden",
                column: "WinKundeListId",
                principalTable: "WinKundeListe",
                principalColumn: "Id");
        }
    }
}
