using DBAccessLib.Core;
using DBAccessLib.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
