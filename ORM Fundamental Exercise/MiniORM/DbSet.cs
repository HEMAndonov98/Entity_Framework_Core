using System.Collections;

namespace MiniORM
{  
    public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        internal ChangeTracker<TEntity>? ChangeTracker { get; set; }

        internal IList<TEntity>? Entities { get; set; }

        public int Count => this.Entities!.Count();

        public bool IsReadOnly => this.Entities!.IsReadOnly;

        internal DbSet(IEnumerable<TEntity> entities)
        {
            this.Entities = entities.ToList();
            this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(ExceptionMessages.NullEntityExceptionMessage);
            }

            this.Entities!.Add(entity);
            this.ChangeTracker!.Add(entity);
        }

        public void Clear()
        {
            while (this.Entities!.Any())
            {
                var entity = this.Entities!.First();
                this.Remove(entity);
            }
        }

        public bool Contains(TEntity entity)
            => this.Entities!.Contains(entity);

        public void CopyTo(TEntity[] entities, int entitiesIndex)
            => this.Entities!.CopyTo(entities, entitiesIndex);

        public bool Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(ExceptionMessages.NullEntityExceptionMessage);
            }

            bool isRemoved = this.Entities.Remove(entity);

            if (isRemoved == true)
            {
                this.ChangeTracker.Remove(entity);
            }

            return isRemoved;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }
        }

        public IEnumerator<TEntity> GetEnumerator()
            => this.Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
