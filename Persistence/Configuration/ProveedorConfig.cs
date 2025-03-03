using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Persistence.Configuration
{
    public class ProveedorConfig : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("proveedor");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.telefono)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.correo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x=> x.direccion)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ruc)
                .IsRequired()
                .HasMaxLength(13);

            builder.Property(x => x.Created)
                .HasColumnType("datetime") // ⚠️ Si `Created` no es string en la BD
                .HasDefaultValueSql("GETDATE()");


            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(30)
                .IsRequired(false);

        }
    }
}
