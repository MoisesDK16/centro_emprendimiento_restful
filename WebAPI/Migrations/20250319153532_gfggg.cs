using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class gfggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion");

            migrationBuilder.AddForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion",
                column: "NegocioId",
                principalTable: "negocio",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion");

            migrationBuilder.AddForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion",
                column: "NegocioId",
                principalTable: "negocio",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
