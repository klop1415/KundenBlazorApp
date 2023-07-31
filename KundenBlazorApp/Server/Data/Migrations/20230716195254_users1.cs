using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class users1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarName",
                table: "AspNetUsers",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kunden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name1 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name2 = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numer = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true),
                    CreatDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kunden_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kunden_UserId",
                table: "Kunden",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kunden");

            migrationBuilder.DropColumn(
                name: "AvatarName",
                table: "AspNetUsers");
        }
    }
}
