using Microsoft.EntityFrameworkCore.Migrations;

namespace IepProjekat.Migrations
{
    public partial class kurac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "618ca4f6-ec19-476e-af89-9cb0b47ea9f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cda3736-4747-47ca-998b-ee7e9a076282");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1959561f-99d5-47b5-a004-d7d6eb9e28bb", "ecf2edc1-bab5-4bc6-85d1-58188134e790", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35d9ce48-590c-4b0c-b15c-eefd07c38ab2", "6904ebbf-e2f8-4e8b-b7e9-86afa5cf1f71", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1959561f-99d5-47b5-a004-d7d6eb9e28bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35d9ce48-590c-4b0c-b15c-eefd07c38ab2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8cda3736-4747-47ca-998b-ee7e9a076282", "6500f18b-064f-4a1b-b0a4-eba7fbd00780", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "618ca4f6-ec19-476e-af89-9cb0b47ea9f1", "28bb0fc8-7f77-4eaa-bad9-a9f2ee846032", "Administrator", "ADMINISTRATOR" });
        }
    }
}
