using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixStockRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Verificar si la columna StockId ya existe antes de agregarla
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = 'detalle' AND COLUMN_NAME = 'StockId')
        BEGIN
            ALTER TABLE detalle ADD StockId BIGINT NULL DEFAULT CAST(0 AS BIGINT);
        END
    ");

            // Verificar si el índice ya existe antes de crearlo
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                       WHERE name = 'IX_detalle_StockId' AND object_id = OBJECT_ID('detalle'))
        BEGIN
            CREATE INDEX IX_detalle_StockId ON detalle (StockId);
        END
    ");

            migrationBuilder.AddForeignKey(
                name: "FK_detalle_stock_StockId",
                table: "detalle",
                column: "StockId",
                principalTable: "stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Verificar si la clave foránea existe antes de eliminarla
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_detalle_stock_StockId')
        BEGIN
            ALTER TABLE detalle DROP CONSTRAINT FK_detalle_stock_StockId;
        END
    ");

            // Verificar si el índice existe antes de eliminarlo
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_detalle_StockId' AND object_id = OBJECT_ID('detalle'))
        BEGIN
            DROP INDEX IX_detalle_StockId ON detalle;
        END
    ");

            // Eliminar la columna solo si existe
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'detalle' AND COLUMN_NAME = 'StockId')
        BEGIN
            ALTER TABLE detalle DROP COLUMN StockId;
        END
    ");
        }
    }
}
