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
    }
}
