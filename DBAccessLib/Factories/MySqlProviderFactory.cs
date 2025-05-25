using DBAccessLib.Core;
using DBAccessLib.MySql;

namespace DBAccessLib.Factories
{
    public class MySqlProviderFactory: IDbProviderFactory
    {
        public IDatabaseConnection CreateConnection(string connectionString)
           => new MySqlDbConnection(connectionString);

        public IRepository CreateRepository(IDatabaseConnection connection)
            => new MySqlDbRepository(connection);
    }
}
