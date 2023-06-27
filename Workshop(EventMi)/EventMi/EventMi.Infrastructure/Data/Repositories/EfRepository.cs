using EventMi.Infrastructure.Common.RepositoryContracts;

namespace EventMi.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;


    using Microsoft.EntityFrameworkCore;
/// <summary>
/// Implementation of Repository for relational database engine
/// </summary>
/// <typeparam name="TEntity"></typeparam>
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(EventMiDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Representation of a table in the database
        /// </summary>
        protected DbSet<TEntity> DbSet { get; set; }

        /// <summary>
        /// Entity framework DB Context holding information about connection and entity states
        /// </summary>
        protected EventMiDbContext Context { get; set; }

        /// <summary>
        /// Method for getting all entries from a table in DB
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> All() => this.DbSet;

        /// <summary>
        /// Method for detaching and retrieving all entries from a table in the DB
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        /// <summary>
        /// Asynchronous method for adding an entry to the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        /// <summary>
        /// Method for updating an entry in the DB
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// Hard delete method for removing an entry from the Db
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        /// <summary>
        /// Base save changes method
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }
    }
}
