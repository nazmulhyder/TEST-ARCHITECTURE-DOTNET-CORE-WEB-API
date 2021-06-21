using Microsoft.EntityFrameworkCore.Migrations;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Migrations
{
    public partial class addRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88b979a6-6e19-4ee6-a84e-1c9e32090111", "1e4bd71a-fac4-4925-9d01-1effa63f1adc", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ae611b85-ed2c-4294-9d98-6eb201340a8a", "339b740c-c4e3-42cf-b812-09342ccb42e0", "Admin", "ADMIN " });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88b979a6-6e19-4ee6-a84e-1c9e32090111");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae611b85-ed2c-4294-9d98-6eb201340a8a");
        }
    }
}
