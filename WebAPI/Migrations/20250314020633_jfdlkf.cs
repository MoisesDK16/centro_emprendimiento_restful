using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class jfdlkf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "PromocionId",
                table: "producto",
                type: "bigint",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "promocion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CantidadCompra = table.Column<int>(type: "int", nullable: true),
                    CantidadGratis = table.Column<int>(type: "int", maxLength: 5, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CASE WHEN DATEDIFF(DAY, GETDATE(), FechaFin) <= 0 THEN 0 ELSE 1 END")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocion", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_producto_PromocionId",
                table: "producto",
                column: "PromocionId");

            migrationBuilder.AddForeignKey(
                name: "FK_producto_promocion_PromocionId",
                table: "producto",
                column: "PromocionId",
                principalTable: "promocion",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_producto_promocion_PromocionId",
                table: "producto");

            migrationBuilder.DropTable(
                name: "promocion");

            migrationBuilder.DropIndex(
                name: "IX_producto_PromocionId",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "PromocionId",
                table: "producto");
        }
    }
}
