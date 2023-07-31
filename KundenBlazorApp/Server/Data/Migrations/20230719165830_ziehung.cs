using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ziehung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45511014-6fd9-480d-9327-6f3a281f1b2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f0c0b68-1f47-483c-8e60-385628a74121");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd6599f7-693d-4724-9ed1-ff5a5bd68784");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c87d3b60-09fc-492d-915f-c2d8bd9f86d8");

            migrationBuilder.AddColumn<int>(
                name: "KundeListId",
                table: "Kunden",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinKundeListId",
                table: "Kunden",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KundeListe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KundeListe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WinKundeListe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinKundeListe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZiehungAuslosungen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KundeListId = table.Column<int>(type: "int", nullable: true),
                    WinKundeListId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZiehungAuslosungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZiehungAuslosungen_KundeListe_KundeListId",
                        column: x => x.KundeListId,
                        principalTable: "KundeListe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZiehungAuslosungen_WinKundeListe_WinKundeListId",
                        column: x => x.WinKundeListId,
                        principalTable: "WinKundeListe",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kunden_KundeListId",
                table: "Kunden",
                column: "KundeListId");

            migrationBuilder.CreateIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden",
                column: "WinKundeListId");

            migrationBuilder.CreateIndex(
                name: "IX_ZiehungAuslosungen_KundeListId",
                table: "ZiehungAuslosungen",
                column: "KundeListId");

            migrationBuilder.CreateIndex(
                name: "IX_ZiehungAuslosungen_WinKundeListId",
                table: "ZiehungAuslosungen",
                column: "WinKundeListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kunden_KundeListe_KundeListId",
                table: "Kunden",
                column: "KundeListId",
                principalTable: "KundeListe",
                principalColumn: "Id");

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
                name: "FK_Kunden_KundeListe_KundeListId",
                table: "Kunden");

            migrationBuilder.DropForeignKey(
                name: "FK_Kunden_WinKundeListe_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropTable(
                name: "ZiehungAuslosungen");

            migrationBuilder.DropTable(
                name: "KundeListe");

            migrationBuilder.DropTable(
                name: "WinKundeListe");

            migrationBuilder.DropIndex(
                name: "IX_Kunden_KundeListId",
                table: "Kunden");

            migrationBuilder.DropIndex(
                name: "IX_Kunden_WinKundeListId",
                table: "Kunden");

            migrationBuilder.DropColumn(
                name: "KundeListId",
                table: "Kunden");

            migrationBuilder.DropColumn(
                name: "WinKundeListId",
                table: "Kunden");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45511014-6fd9-480d-9327-6f3a281f1b2c", null, "user", "USER" },
                    { "4f0c0b68-1f47-483c-8e60-385628a74121", null, "pichugina", "PICHUGINA" },
                    { "bd6599f7-693d-4724-9ed1-ff5a5bd68784", null, "admin", "ADMIN" },
                    { "c87d3b60-09fc-492d-915f-c2d8bd9f86d8", null, "creator", "CREATOR" }
                });
        }
    }
}
