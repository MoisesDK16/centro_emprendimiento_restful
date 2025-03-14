using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ghdgfgf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NegocioId",
                table: "promocion",
                type: "bigint",
                nullable: true); // ✅ Se permite NULL para evitar problemas al eliminar negocio

            migrationBuilder.CreateIndex(
                name: "IX_promocion_NegocioId",
                table: "promocion",
                column: "NegocioId");

            migrationBuilder.AddForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion",
                column: "NegocioId",
                principalTable: "negocio",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction); // ✅ Si se elimina el negocio, NegocioId en promocion se vuelve NULL
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_promocion_negocio_NegocioId",
                table: "promocion");

            migrationBuilder.DropIndex(
                name: "IX_promocion_NegocioId",
                table: "promocion");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "promocion");
        }
    }
}
