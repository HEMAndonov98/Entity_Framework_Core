namespace Blog.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Blog.Data.Common.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => DbSet.AsNoTracking();

        public virtual IQueryable<TEntity> AllIncludingAsNoTracking<TProperty>(Expression<Func<TEntity, TProperty>> expression)
            => this.DbSet.Include(expression).AsNoTracking();

        public async Task<TEntity> FindAsyncIncluding<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression,
            Expression<Func<TEntity, bool>> filter)
            => await this.DbSet.Include(includeExpression).FirstOrDefaultAsync(filter);

        public virtual Task AddAsync(TEntity entity) => DbSet.AddAsync(entity).AsTask();

        public virtual void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }
    }
}
