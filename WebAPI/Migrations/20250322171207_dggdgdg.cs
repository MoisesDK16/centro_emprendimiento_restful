using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class dggdgdg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalle_stock_StockId",
                table: "detalle");

            migrationBuilder.CreateTable(
                name: "Historial_Stock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<long>(type: "bigint", nullable: false),
                    NegocioId = table.Column<long>(type: "bigint", nullable: false),
                    FechaCorte = table.Column<DateOnly>(type: "date", nullable: false),
                    Existencias = table.Column<int>(type: "int", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historial_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historial_Stock_negocio_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "negocio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Historial_Stock_producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historial_Stock_NegocioId",
                table: "Historial_Stock",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Historial_Stock_ProductoId",
                table: "Historial_Stock",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_detalle_stock_StockId",
                table: "detalle",
                column: "StockId",
                principalTable: "stock",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalle_stock_StockId",
                table: "detalle");

            migrationBuilder.DropTable(
                name: "Historial_Stock");

            migrationBuilder.AddForeignKey(
                name: "FK_detalle_stock_StockId",
                table: "detalle",
                column: "StockId",
                principalTable: "stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
