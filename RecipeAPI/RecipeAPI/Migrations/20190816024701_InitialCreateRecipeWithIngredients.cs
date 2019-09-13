using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class InitialCreateRecipeWithIngredients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Credit = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    PublicState = table.Column<string>(nullable: true),
                    RawText = table.Column<string>(nullable: true),
                    PrepTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Quantity = table.Column<string>(nullable: true),
                    SubQuantity = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Container = table.Column<string>(nullable: true),
                    Descriptor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => new { x.Index, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
