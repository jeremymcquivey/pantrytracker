using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class introduceGroceryList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroceryListItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PantryId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    VarietyId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryListItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroceryListItems_Varieties_VarietyId",
                        column: x => x.VarietyId,
                        principalTable: "Varieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryListItems_ProductId",
                table: "GroceryListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryListItems_VarietyId",
                table: "GroceryListItems",
                column: "VarietyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroceryListItems");
        }
    }
}
