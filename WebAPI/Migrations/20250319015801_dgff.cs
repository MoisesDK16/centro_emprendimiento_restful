using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class dgff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalle_venta_VentaId",
                table: "detalle");

            migrationBuilder.AddForeignKey(
                name: "FK_detalle_venta_VentaId",
                table: "detalle",
                column: "VentaId",
                principalTable: "venta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalle_venta_VentaId",
                table: "detalle");

            migrationBuilder.AddForeignKey(
                name: "FK_detalle_venta_VentaId",
                table: "detalle",
                column: "VentaId",
                principalTable: "venta",
                principalColumn: "Id");
        }
    }
}
