using DBAccessLib.Core;
using DBAccessLib.Sqlite;

namespace DBAccessLib.Factories
{
    public class SqliteProviderFactory : IDbProviderFactory
    {
        public IDatabaseConnection CreateConnection(string connectionString)
           => new SqliteDbConnection(connectionString);

        public IRepository CreateRepository(IDatabaseConnection connection)
            => new SqliteDbRepository(connection);
    }
}
