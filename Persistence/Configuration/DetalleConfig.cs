using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class DetalleConfig : IEntityTypeConfiguration<Detalle>
    {
        public void Configure(EntityTypeBuilder<Detalle> builder)
        {
            builder.ToTable("detalle");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Precio)
                .IsRequired().HasPrecision(10, 2);

            builder.Property(x => x.Cantidad).IsRequired();
            builder.Property(x => x.Total).
                IsRequired().HasPrecision(10,2);

            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.VentaId).IsRequired();

            builder.HasOne(x => x.Producto)
                .WithMany().HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Venta)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Stock)
                .WithMany(s => s.Detalles) 
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(x => x.Promocion).WithMany(x => x.Detalles)
                .HasForeignKey(x => x.PromocionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
