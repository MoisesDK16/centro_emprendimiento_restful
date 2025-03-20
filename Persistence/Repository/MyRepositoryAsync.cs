using Application.Exceptions;
using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T>, IReadOnlyRepositoryAsync<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork; // ✅ Agregamos UnitOfWork

        public MyRepositoryAsync(AppDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbContext.Set<T>().AddAsync(entity);
            return result.Entity; // ⛔ NO LLAMAR `SaveChangesAsync()`
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask; // ⛔ NO LLAMAR `SaveChangesAsync()`
        }

        public Task<bool> DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.FromResult(true); // ⛔ NO LLAMAR `SaveChangesAsync()`
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask; // ⛔ NO LLAMAR `SaveChangesAsync()`
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChangesAsync(); // ✅ Delegamos a `UnitOfWork`
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
