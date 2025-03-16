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
    public class NegocioClienteConfig : IEntityTypeConfiguration<NegocioCliente>
    {
        public void Configure(EntityTypeBuilder<NegocioCliente> builder)
        {
            builder.HasKey(nc => new { nc.NegocioId, nc.ClienteId });

            builder.HasOne(nc => nc.Negocio)
                   .WithMany(n => n.NegocioClientes)
                   .HasForeignKey(nc => nc.NegocioId);

            builder.HasOne(nc => nc.Cliente)
                   .WithMany(c => c.NegocioClientes)
                   .HasForeignKey(nc => nc.ClienteId);
        }
    }
}
