using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

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
                        catch (SqlException)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }

                        await transaction.CommitAsync();
                    }
                }
            }
        }

        private void Persist<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var currentTableName = this.GetTableName(typeof(TEntity));
            var columns = this._connection.FetchColumnNames(currentTableName).ToArray();

            if (dbSet.ChangeTracker.Added.Any())
            {
                this._connection.InsertEntities(dbSet.ChangeTracker.Added, currentTableName, columns);
            }

            var modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();
            if (modifiedEntities.Any())
            {
                this._connection.UpdateEntities(modifiedEntities, currentTableName, columns);
            }

            var removedEntities = dbSet.ChangeTracker.Removed.ToArray();
            if (removedEntities.Any())
            {
                this._connection.DeleteEntities(removedEntities, currentTableName, columns);
            }
        }

        private void InitializeDbSets()
        {
            foreach (var dbSet in this._dbSetProperties)
            {
                var dbSetType = dbSet.Key;
                var dbSetProperty = dbSet.Value;

                var populateDbsetGeneric = this.GetType()
                    .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                populateDbsetGeneric.Invoke(this, new object[] { dbSetProperty });
            }
        }

        private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
            where TEntity : class, new()
        {
            var entities = LoadTableEntities<TEntity>();
            var dbSetInsance = new DbSet<TEntity>(entities);
            ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInsance);

        }

        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class, new()
        {
            var table = typeof(TEntity);
            string[] columns = this.GetEntityColumns(table);
            string tableName = this.GetTableName(table);
            var fetchedRows = this._connection.FetchResultSet<TEntity>(tableName, columns).ToArray();
            return fetchedRows;
        }

        private string[] GetEntityColumns(Type table)
        {
            var tableName = this.GetTableName(table);
            var dbColumns = this._connection.FetchColumnNames(tableName);

            var columns = table.GetProperties()
                .Where(pi => dbColumns.Contains(pi.Name) &&
                !pi.HasAttribute<NotMappedAttribute>() &&
                AllowedSqlTypes.Contains(pi.PropertyType))
                .Select(pi => pi.Name)
                .ToArray();

            return columns;
        }

        private string GetTableName(Type type)
        {
            var tableName = type.Name;
            if (tableName == null)
            {
                tableName = this._dbSetProperties[type].Name;
            }

            return tableName;
        }

        private bool IsObjectValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationErrors = new List<ValidationResult>();
            bool validationResult = Validator.TryValidateObject(entity, validationContext, validationErrors, true);
            return validationResult;
        }

        private void MapAllRelations()
        {
            foreach (var dbSetProperty in this._dbSetProperties)
            {
                var dbSetType = dbSetProperty.Key;
                var mapRelationsMethodGeneric = this.GetType()
                    .GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                var dbSet = dbSetProperty.Value;

                mapRelationsMethodGeneric.Invoke(this, new object[] { dbSet });
            }
        }

        private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var entityType = typeof(TEntity);
            this.MapNavigationProperties(dbSet);
            var collections = entityType.GetProperties()
                .Where(pi => pi.PropertyType.IsGenericType &&
                pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection))
                .ToArray();

            foreach (var collection in collections)
            {
                var collectionType = collection.PropertyType.GetGenericArguments().First();
                var mapCollectionMethodGeneric = collectionType
                    .GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType, collectionType);

                mapCollectionMethodGeneric.Invoke(this, new object[] { dbSet, collection });
            }
        }

        private void MapCollectiion<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collectionProperty)
            where TDbSet : class, new()
            where TCollection : class, new()
        {
            var entityType = typeof(TDbSet);
            var collectionType = typeof(TCollection);

            var primaryKeys = collectionType.GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            var primaryKey = primaryKeys.First();
            var foreignKey = entityType.GetProperties().First(pi => pi.HasAttribute<KeyAttribute>());

            var isManyToMany = primaryKeys.Length > 2;
            if (isManyToMany)
            {
                primaryKey = collectionType.GetProperties()
                    .First(pi => collectionType
                        .GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
                        .PropertyType == entityType);

                var navigationDbSet = (DbSet<TCollection>)this._dbSetProperties[collectionType].GetValue(this);

                foreach (var entity in dbSet)
                {
                    var primaryKeyValue = foreignKey.GetValue(entity);
                    var navigationEntities = navigationDbSet
                        .Where(navigationEntity => primaryKey.GetValue(navigationEntity).Equals(primaryKeyValue))
                        .ToArray();

                    ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);
                }
            }
        }

        private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var entityType = typeof(TEntity);
            PropertyInfo[] foreignKeys = entityType
                .GetProperties()
                .Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
                .ToArray();

            foreach (var foreignKey in foreignKeys)
            {
                string navigationPropertyName = foreignKey.GetCustomAttribute<ForeignKeyAttribute>().Name;
                PropertyInfo navigationProperty = entityType.GetProperty(navigationPropertyName);

                var navigationDbSet = this._dbSetProperties[navigationProperty.PropertyType]
                    .GetValue(this);

                var navigationPrimaryKey = navigationProperty.PropertyType.GetProperties().First(pi => pi.HasAttribute<ForeignKeyAttribute>());

                foreach (var entity in dbSet)
                {
                    var foreignKeyValue = foreignKey.GetValue(entity);

                    var navigationPropertyValue = ((IEnumerable<object>)navigationDbSet).First(currentNavigationProperty => navigationPrimaryKey.GetValue(currentNavigationProperty).Equals(foreignKeyValue));


                    navigationProperty.SetValue(entity, navigationPropertyValue);
                }
            }
        }

        private IDictionary<Type, PropertyInfo>? DiscoverDbSets()
        {
            var dbSets = this.GetType().GetProperties()
                .Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);

            return dbSets;
        }
    }
}
