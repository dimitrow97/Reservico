using Microsoft.EntityFrameworkCore;
using Reservico.Data.Interfaces;
using System.Linq.Expressions;

namespace Reservico.Data
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ReservicoDbContext dbContext;
        private readonly DbSet<T> entities;

        public Repository(ReservicoDbContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
            entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await this.entities.FindAsync(id);
        }

        public async Task AddAsync(T entity, bool saveChanges = true)
        {
            await this.entities.AddAsync(entity);

            if (saveChanges)
            {
                await this.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T entity, bool saveChanges = true)
        {          
            this.entities.Attach(entity);
            this.dbContext.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
            {
                await this.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.dbContext
                .SaveChangesAsync()
                .ConfigureAwait(false) > 0;
        }

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.entities
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FindByConditionAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includePaths)
        {
            var query = includePaths.Aggregate(
                this.entities.AsQueryable(),
                (current, item) => EvaluateInclude(current, item));

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.entities.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllByConditionAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includePaths)
        {
            var query = includePaths.Aggregate(
                this.entities.AsQueryable(),
                (current, item) => EvaluateInclude(current, item));

            return await query.Where(predicate).ToListAsync();
        }

        public IQueryable<T> Query()
            => this.entities.AsQueryable();

        public async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            this.entities.Remove(entity);

            if (saveChanges)
            {
                await this.SaveChangesAsync();
            }
        }

        private IQueryable<T> EvaluateInclude(IQueryable<T> current, Expression<Func<T, object>> item)
        {
            if (item.Body is MethodCallExpression)
            {
                var arguments = ((MethodCallExpression)item.Body).Arguments;
                if (arguments.Count > 1)
                {
                    var navigationPath = string.Empty;
                    for (var i = 0; i < arguments.Count; i++)
                    {
                        var arg = arguments[i];
                        var path = arg.ToString()
                            .Substring(arg.ToString().IndexOf('.') + 1);

                        navigationPath += (i > 0 ? "." : string.Empty) + path;
                    }
                    return current.Include(navigationPath);
                }
            }

            return current.Include(item);
        }
    }
}
