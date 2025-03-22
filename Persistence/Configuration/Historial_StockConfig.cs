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
    public class Historial_StockConfig : IEntityTypeConfiguration<Historial_Stock>
    {
        public void Configure(EntityTypeBuilder<Historial_Stock> builder)
        {
            builder.ToTable("Historial_Stock");
            builder.HasKey(x => x.Id);
        }
    }
}
