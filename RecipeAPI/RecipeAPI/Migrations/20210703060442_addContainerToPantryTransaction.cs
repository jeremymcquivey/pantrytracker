using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class addContainerToPantryTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Container",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Container",
                table: "GroceryListItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Container",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Container",
                table: "GroceryListItems");
        }
    }
}
