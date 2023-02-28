using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MiniORM
{
    public class ChangeTracker<T>
        where T : class, new()
    {
        private readonly IList<T>? _allEntities;
        private readonly IList<T>? _added;
        private readonly IList<T>? _removed;

        public IReadOnlyCollection<T> AllEntities =>  this._allEntities!.ToList().AsReadOnly();
        public IReadOnlyCollection<T> Added => this._added!.ToList().AsReadOnly();
        public IReadOnlyCollection<T> Removed => this._removed!.ToList().AsReadOnly();

        private ChangeTracker()
        {
            this._added = new List<T>();
            this._removed = new List<T>();
        }

        public ChangeTracker(IEnumerable<T> entities)
            : base()
        {
            this._allEntities = CloneEntities(entities);
        }

        private IList<T>? CloneEntities(IEnumerable<T> entities)
        {
            var clonedEntities = new List<T>();
            var propertiesToClone = typeof(T).GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes!.Contains(pi.PropertyType))
                .ToArray();

            foreach (var entity in entities)
            {
                var clonedEntity = Activator.CreateInstance<T>();
                foreach (var property in propertiesToClone)
                {
                    var value = property.GetValue(entity);
                    property.SetValue(clonedEntity, value);
                }
                clonedEntities.Add(clonedEntity);
            }
            return clonedEntities;
        }

        public void Add(T enttity) => this._added.Add(enttity);

        public void Remove(T entity) => this._removed.Add(entity);

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            var modifiedEntities = new List<T>();
            var primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var proxyEntitiy in AllEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntitiy);
                var entity = dbSet.Entities.Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                bool isModified = IsModified(proxyEntitiy, entity);

                if (isModified)
                {
                    modifiedEntities.Add(proxyEntitiy);
                }
            }

            return modifiedEntities;
        }

        private bool IsModified(T proxyEntitiy, T entity)
        {
            var monitoredProperties = typeof(T).GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
                .ToArray();

            var modifiedProperties = monitoredProperties.Where(pi => !Equals(pi.GetValue(proxyEntitiy), pi.GetValue(entity))).ToArray();

            return modifiedProperties.Any();
        }

        private static IEnumerable<object> GetPrimaryKeyValues(PropertyInfo[] primaryKeys, T proxyEntitiy)
            => primaryKeys.Select(pk => pk.GetValue(proxyEntitiy));
    }
}