using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixIdToBigInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_proveedor",
                table: "proveedor");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "proveedor",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ID_PROVEEDOR",
                table: "proveedor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_proveedor",
                table: "proveedor",
                column: "ID_PROVEEDOR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_proveedor",
                table: "proveedor");

            migrationBuilder.DropColumn(
                name: "ID_PROVEEDOR",
                table: "proveedor");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "proveedor",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_proveedor",
                table: "proveedor",
                column: "Id");
        }
    }
}
