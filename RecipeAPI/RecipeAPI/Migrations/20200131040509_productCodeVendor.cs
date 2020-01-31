using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class productCodeVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Source",
                table: "ProductCodes",
                newName: "VendorCode");

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "ProductCodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "ProductCodes");

            migrationBuilder.RenameColumn(
                name: "VendorCode",
                table: "ProductCodes",
                newName: "Source");
        }
    }
}
