using System.Reflection;

namespace MiniORM
{
    public abstract class DbContext
    {
        //This will be our connection to the db.
        private readonly DatabaseConnection _connection;

        // Here we will store all our dbSets.
        private readonly IDictionary<Type, PropertyInfo> _dbSetProperties;

        internal static readonly Type[] AllowedSqlTypes =
        {
            typeof(string),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(bool),
            typeof(DateTime),
        };

        protected DbContext(string connectionString)
        {
            this._connection = new DatabaseConnection(connectionString);
            this._dbSetProperties = this.DiscoverDbSets();

            using (new ConnectionManager(this._connection))
            {
                this.InitializeDbSets();
            }
            this.MapAllRelations();
        }

        public async Task SaveChanges()
        {
            var dbSets = _dbSetProperties
                .Select(pi => pi.Value.GetValue(this))
                .ToArray();

            foreach (IEnumerable<object> dbSet in dbSets)
            {
                var invalidEntities = dbSet.Where(entity => !this.IsObjectValid(entity)).ToArray();

                if (invalidEntities.Any())
                {
                    throw new InvalidOperationException(string.Format(ExceptionMessages.NonExistingEntitiesExceptionMessage, invalidEntities.Length, dbSet.GetType().Name));
                }
            }

            using (new ConnectionManager(this._connection))
            {
                using (var transaction = this._connection.StartTransaction())
                {
                    foreach (IEnumerable<object> dbSet in dbSets)
                    {
                        var dbSetType = dbSet.GetType().GetGenericArguments().First();
                        var persistMethod = typeof(DbContext)
                            .GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(dbSetType);

                        try
                        {
                            persistMethod.Invoke(this, new object[] { dbSet });
                        }
                        catch (TargetInvocationException tie)
                        {
                            throw tie.InnerException;
                        }
                        catch (InvalidOperationException)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
        }

        private bool IsObjectValid(object entity)
        {
            throw new NotImplementedException();
        }

        private void MapAllRelations()
        {
            throw new NotImplementedException();
        }

        private void InitializeDbSets()
        {
            throw new NotImplementedException();
        }

        private IDictionary<Type, PropertyInfo>? DiscoverDbSets()
        {
            throw new NotImplementedException();
        }
    }
}
