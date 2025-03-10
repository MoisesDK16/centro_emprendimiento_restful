using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>,  IRepositoryAsync<T>, IReadOnlyRepositoryAsync<T> where T : class
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public MyRepositoryAsync(IDbContextFactory<AppDbContext> contextFactory) : base(contextFactory.CreateDbContext())
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> AddAsync(T entity)
        {
            await using var dbContext = _contextFactory.CreateDbContext();
            var result = await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }


        public async Task<T> GetByIdAsync(long id)
        {
            await using var dbContext = _contextFactory.CreateDbContext();

            // Obtener todas las propiedades de navegación dinámicamente y aplicar Include()
            IQueryable<T> query = dbContext.Set<T>();
            foreach (var includeExpression in GetAllNavigationProperties<T>(dbContext))
            {
                query = query.Include(includeExpression);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }



        public async Task UpdateAsync(T entity)
        {
            await using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            await using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            await using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private static List<Expression<Func<T, object>>> GetAllNavigationProperties<T>(DbContext dbContext) where T : class
        {
            var entityType = dbContext.Model.FindEntityType(typeof(T));

            if (entityType == null)
                return new List<Expression<Func<T, object>>>();

            return entityType.GetNavigations()
                .Select(navigation =>
                {
                    var parameter = Expression.Parameter(typeof(T), "e");
                    var property = Expression.Property(parameter, navigation.Name);

                    // NO usamos Convert a object, EF Core puede trabajar sin esto
                    return Expression.Lambda<Func<T, object>>(property, parameter);
                })
                .ToList();
        }


    }
}
