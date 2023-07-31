using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KundenBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
