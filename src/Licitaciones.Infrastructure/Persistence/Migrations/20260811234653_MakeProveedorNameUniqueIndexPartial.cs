using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licitaciones.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeProveedorNameUniqueIndexPartial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proveedores_NombreNormalizado",
                table: "Proveedores");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_NombreNormalizado",
                table: "Proveedores",
                column: "NombreNormalizado",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proveedores_NombreNormalizado",
                table: "Proveedores");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_NombreNormalizado",
                table: "Proveedores",
                column: "NombreNormalizado",
                unique: true);
        }
    }
}
