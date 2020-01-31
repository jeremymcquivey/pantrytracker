using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class productCodeSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "ProductCodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "ProductCodes");
        }
    }
}
