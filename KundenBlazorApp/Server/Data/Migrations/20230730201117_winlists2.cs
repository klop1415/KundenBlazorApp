using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class winlists2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZiehungAuslosungen_WinKundeListe_WinKundeListId",
                table: "ZiehungAuslosungen");

            migrationBuilder.DropTable(
                name: "KundeWinKundeList");

            migrationBuilder.DropIndex(
                name: "IX_ZiehungAuslosungen_WinKundeListId",
                table: "ZiehungAuslosungen");

            migrationBuilder.DropColumn(
                name: "WinKundeListId",
                table: "ZiehungAuslosungen");

            migrationBuilder.AddColumn<int>(
                name: "WinKundeListId",
                table: "Kunden",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KundeZiehungAuslosung",
                columns: table => new
                {
                    WinKundeListId = table.Column<int>(type: "int", nullable: false),
                    WinKundeListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KundeZiehungAuslosung", x => new { x.WinKundeListId, x.WinKundeListsId });
                    table.ForeignKey(
                        name: "FK_KundeZiehungAuslosung_Kunden_WinKundeListId",
                        column: x => x.WinKundeListId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KundeZiehungAuslosung_ZiehungAuslosungen_WinKundeListsId",
                        column: x => x.WinKundeListsId,
                        principalTable: "ZiehungAuslosungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden",
                column: "WinKundeListId");

            migrationBuilder.CreateIndex(
                name: "IX_KundeZiehungAuslosung_WinKundeListsId",
                table: "KundeZiehungAuslosung",
                column: "WinKundeListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kunden_WinKundeListe_WinKundeListId",
                table: "Kunden",
                column: "WinKundeListId",
                principalTable: "WinKundeListe",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kunden_WinKundeListe_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropTable(
                name: "KundeZiehungAuslosung");

            migrationBuilder.DropIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropColumn(
                name: "WinKundeListId",
                table: "Kunden");

            migrationBuilder.AddColumn<int>(
                name: "WinKundeListId",
                table: "ZiehungAuslosungen",
                type: "int",
                nullable: true);

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
                name: "IX_ZiehungAuslosungen_WinKundeListId",
                table: "ZiehungAuslosungen",
                column: "WinKundeListId");

            migrationBuilder.CreateIndex(
                name: "IX_KundeWinKundeList_WinKundeListsId",
                table: "KundeWinKundeList",
                column: "WinKundeListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZiehungAuslosungen_WinKundeListe_WinKundeListId",
                table: "ZiehungAuslosungen",
                column: "WinKundeListId",
                principalTable: "WinKundeListe",
                principalColumn: "Id");
        }
    }
}
