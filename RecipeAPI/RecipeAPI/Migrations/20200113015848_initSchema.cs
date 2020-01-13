using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class initSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Credit = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    PublicState = table.Column<string>(nullable: true),
                    RawText = table.Column<string>(nullable: true),
                    PrepTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => new { x.RecipeId, x.Index });
                    table.ForeignKey(
                        name: "FK_Directions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    SubQuantity = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Container = table.Column<string>(nullable: true),
                    Descriptor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => new { x.RecipeId, x.Index });
                    table.ForeignKey(
                        name: "FK_Ingredients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
