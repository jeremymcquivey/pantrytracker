using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class addQuantityAndUnitToListPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "UserProductPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "UserProductPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "UserProductPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UserProductPreferences");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "UserProductPreferences");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "UserProductPreferences");
        }
    }
}
