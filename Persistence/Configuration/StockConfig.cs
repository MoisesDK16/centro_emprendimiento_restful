using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("stock");
            builder.HasKey(s => s.Id);

            builder.Property(x => x.PrecioCompra)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.PrecioVenta)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Cantidad)
                .IsRequired();

            builder.Property(x => x.FechaElaboracion)
                .IsRequired();

            builder.Property(x => x.FechaIngreso)
                .IsRequired();
        }
    }
}
