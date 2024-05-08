using System.Linq.Expressions;

namespace Reservico.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task AddAsync(T entity, bool saveChanges = true);

        Task UpdateAsync(T entity, bool saveChanges = true);

        Task DeleteAsync(T entity, bool saveChanges = true);

        Task<bool> SaveChangesAsync();

        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);

        Task<T> FindByConditionAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includePaths);

        Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> FindAllByConditionAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includePaths);

        IQueryable<T> Query();
    }
}