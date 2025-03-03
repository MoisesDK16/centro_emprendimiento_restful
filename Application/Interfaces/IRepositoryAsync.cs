using Ardalis.Specification;

namespace Application.Interfaces
{
    public interface IRepositoryAsync<T>: IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<T> GetByIdAsync(long Id);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

    }

    public interface IReadOnlyRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {
    }
}
