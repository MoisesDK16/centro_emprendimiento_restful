using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

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
            var entity = await dbContext.Set<T>().FindAsync(new object[] { id });
            if (entity == null) throw new KeyNotFoundException("Registro no encontrado");
            return entity;
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

    }
}
