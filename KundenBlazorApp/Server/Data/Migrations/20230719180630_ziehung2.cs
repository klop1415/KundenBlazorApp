using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ziehung2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatDate",
                table: "Kunden",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "KundeListe",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KundeListe_UserId",
                table: "KundeListe",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KundeListe_AspNetUsers_UserId",
                table: "KundeListe",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KundeListe_AspNetUsers_UserId",
                table: "KundeListe");

            migrationBuilder.DropIndex(
                name: "IX_KundeListe_UserId",
                table: "KundeListe");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "KundeListe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatDate",
                table: "Kunden",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
