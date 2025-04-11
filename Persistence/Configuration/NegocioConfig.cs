using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class NegocioConfig : IEntityTypeConfiguration<Negocio>
    {
        public void Configure(EntityTypeBuilder<Negocio> builder)
        {
            builder.ToTable("negocio");

            builder.Property(x => x.nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.direccion)
                .HasMaxLength(100);

            builder.Property(x => x.telefono)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.descripcion)
                .HasMaxLength(1024);
        }
    }
}
