using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licitaciones.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Iteration03OfertasAprobacion : Migration
    {
        private static readonly string[] OfertaUniqueColumns = ["LicitacionId", "ProveedorId"];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NivelesAprobacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoMinimoCrc = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MontoMaximoCrc = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Aprobador = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesAprobacion", x => x.Id);
                    table.CheckConstraint("CK_NivelesAprobacion_MaximoValido", "\"MontoMaximoCrc\" IS NULL OR \"MontoMaximoCrc\" >= \"MontoMinimoCrc\"");
                    table.CheckConstraint("CK_NivelesAprobacion_MinimoPositivo", "\"MontoMinimoCrc\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicitacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProveedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoOfertadoCrc = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaRegistro = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                    table.CheckConstraint("CK_Ofertas_MontoPositivo", "\"MontoOfertadoCrc\" > 0");
                    table.ForeignKey(
                        name: "FK_Ofertas_Licitaciones_LicitacionId",
                        column: x => x.LicitacionId,
                        principalTable: "Licitaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ofertas_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql(
                """
                ALTER TABLE "NivelesAprobacion"
                ADD CONSTRAINT "EX_NivelesAprobacion_SinTraslapes"
                EXCLUDE USING gist (numrange("MontoMinimoCrc", "MontoMaximoCrc", '[]') WITH &&);
                """);

            migrationBuilder.CreateIndex(
                name: "IX_NivelesAprobacion_MontoMinimoCrc",
                table: "NivelesAprobacion",
                column: "MontoMinimoCrc");

            migrationBuilder.CreateIndex(
                name: "IX_NivelesAprobacion_UnicoRangoAbierto",
                table: "NivelesAprobacion",
                column: "MontoMaximoCrc",
                unique: true,
                filter: "\"MontoMaximoCrc\" IS NULL")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_LicitacionId_ProveedorId",
                table: "Ofertas",
                columns: OfertaUniqueColumns,
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_ProveedorId",
                table: "Ofertas",
                column: "ProveedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NivelesAprobacion");

            migrationBuilder.DropTable(
                name: "Ofertas");
        }
    }
}
