using System.Linq.Expressions;

namespace Blog.Data.Common.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        IQueryable<TEntity> AllIncludingAsNoTracking<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        Task<TEntity> FindAsyncIncluding<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression, Expression<Func<TEntity, bool>> filter);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
