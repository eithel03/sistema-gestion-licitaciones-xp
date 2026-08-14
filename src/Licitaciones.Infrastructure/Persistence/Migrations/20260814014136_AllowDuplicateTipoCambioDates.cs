using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licitaciones.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowDuplicateTipoCambioDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TiposCambio_Fecha",
                table: "TiposCambio");

            migrationBuilder.CreateIndex(
                name: "IX_TiposCambio_Fecha",
                table: "TiposCambio",
                column: "Fecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TiposCambio_Fecha",
                table: "TiposCambio");

            migrationBuilder.CreateIndex(
                name: "IX_TiposCambio_Fecha",
                table: "TiposCambio",
                column: "Fecha",
                unique: true);
        }
    }
}
