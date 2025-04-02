using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ParametrosConfig : IEntityTypeConfiguration<Parametros>
    {
        public void Configure(EntityTypeBuilder<Parametros> builder)
        {
            builder.ToTable("parametros");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nombre).IsRequired();
            builder.Property(x => x.Valor)
                .IsRequired()
                .HasPrecision(10, 2); // Agrega esta línea para definir la precisión decimal
        }

    }
}
