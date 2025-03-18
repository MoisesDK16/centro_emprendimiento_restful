using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Contexts
{
    public class IdentityContext: IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Apellido).IsRequired();
                entity.Property(e => e.CiudadOrigen).IsRequired();

                entity.HasIndex(e => e.UserName).IsUnique();

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Identificacion).IsUnique();
            });

        }
    }
}

