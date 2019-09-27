using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryTracker.SingleSignOn.STS.Migrations
{
    public partial class seedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8e91b927-d0bf-4a00-b4e6-8f3abf87481b", "0cc3d1e8-3560-4ff8-b268-f8a30a209bfb", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "229e59a2-f740-484d-ac04-25c6a4851464", "be2c6a3d-70ec-41ee-aa5c-2569838b7e70", "Premium", "PREMIUM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "229e59a2-f740-484d-ac04-25c6a4851464");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e91b927-d0bf-4a00-b4e6-8f3abf87481b");
        }
    }
}
