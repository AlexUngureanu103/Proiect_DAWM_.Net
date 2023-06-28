using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class removedWeights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "PortionSize",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortionSize",
                table: "Recipes");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "RecipeIngredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalWeight",
                table: "Ingredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
