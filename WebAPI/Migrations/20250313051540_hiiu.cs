using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class hiiu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Asegurar que la columna 'NegocioId' existe antes de cualquier índice
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = 'categoria' AND COLUMN_NAME = 'NegocioId')
        BEGIN
            ALTER TABLE categoria ADD NegocioId bigint NOT NULL;
        END;
    ");

            // Verificar si la tabla 'categoria' ya existe antes de intentar crearla
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'categoria')
        BEGIN
            CREATE TABLE [categoria] (
                [Id] bigint NOT NULL IDENTITY,
                [Nombre] nvarchar(20) NOT NULL,
                [Tipo] int NOT NULL,
                [Descripcion] nvarchar(50) NULL,
                [NegocioId] bigint NOT NULL,
                [CreatedBy] nvarchar(max) NULL,
                [Created] datetime2 NULL,
                [LastModifiedBy] nvarchar(max) NULL,
                [LastModified] datetime2 NULL,
                CONSTRAINT [PK_categoria] PRIMARY KEY ([Id]),
                CONSTRAINT [FK_categoria_negocio_NegocioId] FOREIGN KEY ([NegocioId]) 
                    REFERENCES [negocio]([Id]) ON DELETE CASCADE
            );
        END;
    ");

            // Crear el índice solo si la columna NegocioId existe
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'categoria' AND COLUMN_NAME = 'NegocioId')
        BEGIN
            CREATE INDEX IX_categoria_NegocioId ON categoria (NegocioId);
        END;
    ");
        }




        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'categoria')
        BEGIN
            DROP TABLE categoria;
        END;
    ");
        }


    }
}
