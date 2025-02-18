using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddCatalogSchema18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unit",
                schema: "catalog",
                table: "Products",
                newName: "BulkUnit");

            migrationBuilder.AddColumn<string>(
                name: "BaseUnit",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConversionFactor",
                schema: "catalog",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUnit",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ConversionFactor",
                schema: "catalog",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "BulkUnit",
                schema: "catalog",
                table: "Products",
                newName: "Unit");
        }
    }
}
