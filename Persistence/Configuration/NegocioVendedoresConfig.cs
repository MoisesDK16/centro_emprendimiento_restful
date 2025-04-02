using Domain.Entities;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class NegocioVendedoresConfig : IEntityTypeConfiguration<NegocioVendedores>
    {
        public void Configure(EntityTypeBuilder<NegocioVendedores> builder)
        {
            builder.ToTable("negocio_vendedor");
            builder.HasKey(nv => new { nv.NegocioId, nv.VendedorId });
            builder.HasOne(nv => nv.Negocio)
                   .WithMany(n => n.NegocioVendedores)
                   .HasForeignKey(nv => nv.NegocioId);
            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(nv => nv.VendedorId)
                   .IsRequired(false);


        }
    }
}
