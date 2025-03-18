using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    using Domain.Entities;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Persistence.Configuration
    {
        public class ClienteConfig : IEntityTypeConfiguration<Cliente>
        {
            public void Configure(EntityTypeBuilder<Cliente> builder)
            {
                builder.ToTable("cliente");
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Identificacion)
                    .IsRequired()
                    .HasMaxLength(15);

                builder.HasIndex(c => c.Identificacion)
                    .IsUnique();

                builder.Property(c => c.Nombres)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(c => c.PrimerApellido)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(c => c.SegundoApellido)
                    .HasMaxLength(50);

                builder.Property(c => c.Email)
                    .HasMaxLength(50);

                builder.Property(c => c.Telefono)
                    .HasMaxLength(15);

                builder.Property(c => c.Ciudad)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(c => c.Direccion)
                    .HasMaxLength(200);
            }
        }
    }

}
