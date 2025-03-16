using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Context
{
    public class AppDbContext: DbContext
    {

        private readonly IDateTimeService _dateTime;
        public AppDbContext(DbContextOptions<AppDbContext> options, IDateTimeService dateTime) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
        }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Negocio> Negocios { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Producto> productos { get; set; }

        public DbSet<NegocioCliente> NegocioClientes { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Promocion> Promociones { get; set; }

        public DbSet<Venta> Ventas { get; set; }

        public DbSet<Detalle> Detalles { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
