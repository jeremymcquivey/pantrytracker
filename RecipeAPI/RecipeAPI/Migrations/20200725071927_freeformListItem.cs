using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class freeformListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryListItems_Products_ProductId",
                table: "GroceryListItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "GroceryListItems",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FreeformText",
                table: "GroceryListItems",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryListItems_Products_ProductId",
                table: "GroceryListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryListItems_Products_ProductId",
                table: "GroceryListItems");

            migrationBuilder.DropColumn(
                name: "FreeformText",
                table: "GroceryListItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "GroceryListItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryListItems_Products_ProductId",
                table: "GroceryListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
