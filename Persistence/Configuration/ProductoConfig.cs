    using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
namespace Persistence.Configuration
{
    public class ProductoConfig : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("producto");
            builder.HasKey(p => p.Id);

            builder.Property(x => x.Codigo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Descripcion)
            .HasMaxLength(1024);

            builder.Property(p => p.Iva)
                 .HasColumnType("decimal(10,2)");

            builder.Property(x => x.RutaImagen)
                .HasMaxLength(1024);

        }
    }
}
