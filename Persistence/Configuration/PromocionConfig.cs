using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class PromocionConfig : IEntityTypeConfiguration<Promocion>
    {
        public void Configure(EntityTypeBuilder<Promocion> builder)
        {
            builder.ToTable("promocion");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TipoPromocion).IsRequired();
            builder.Property(x => x.FechaInicio).IsRequired();
            builder.Property(x => x.FechaFin).IsRequired();
            builder.Property(x => x.Estado).IsRequired();

            builder.Property(x => x.Descuento)
                .HasPrecision(18, 2)
                .IsRequired(false);

            builder.Property(x => x.CantidadCompra)
                .HasColumnType("int")
                .IsRequired(false); 

            builder.Property(x => x.CantidadGratis)
                .HasColumnType("int")
                .IsRequired(false); 

            builder.HasMany(x => x.Productos)
                .WithOne(x => x.Promocion)
                .HasForeignKey(x => x.PromocionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Negocio)
                .WithMany(n => n.Promociones)
                .HasForeignKey(x => x.NegocioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
