using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria");
            builder.HasKey(c => c.Id);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Descripcion)
                .HasMaxLength(200);

        }
    }
}
