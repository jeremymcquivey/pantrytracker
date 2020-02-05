using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class productVarietyToUPC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "ProductCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCodes_VarietyId",
                table: "ProductCodes",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCodes_Varieties_VarietyId",
                table: "ProductCodes",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCodes_Varieties_VarietyId",
                table: "ProductCodes");

            migrationBuilder.DropIndex(
                name: "IX_ProductCodes_VarietyId",
                table: "ProductCodes");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "ProductCodes");
        }
    }
}
