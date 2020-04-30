using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeAPI.Migrations
{
    public partial class trackProductPreferenceWithVariety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "UserProductPreferences",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProductPreferences_VarietyId",
                table: "UserProductPreferences",
                column: "VarietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VarietyId",
                table: "Transactions",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Varieties_VarietyId",
                table: "Transactions",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProductPreferences_Varieties_VarietyId",
                table: "UserProductPreferences",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Varieties_VarietyId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProductPreferences_Varieties_VarietyId",
                table: "UserProductPreferences");

            migrationBuilder.DropIndex(
                name: "IX_UserProductPreferences_VarietyId",
                table: "UserProductPreferences");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_VarietyId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "UserProductPreferences");
        }
    }
}
