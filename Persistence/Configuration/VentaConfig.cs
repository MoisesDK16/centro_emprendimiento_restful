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
    public class VentaConfig : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("venta");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Fecha).IsRequired();
            builder.Property(x => x.Subtotal).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.Total).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.ClienteId).IsRequired();

            builder.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId);
            builder.HasMany(x => x.Detalles).WithOne(x => x.Venta).HasForeignKey(x => x.VentaId);
        }
    }
}
