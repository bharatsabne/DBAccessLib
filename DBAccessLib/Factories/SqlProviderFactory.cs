using DBAccessLib.Core;
using DBAccessLib.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Factories
{
    public class SqlProviderFactory : IDbProviderFactory
    {
        public IDatabaseConnection CreateConnection(string connectionString)
           => new SqlDbConnection(connectionString);

        public IRepository CreateRepository(IDatabaseConnection connection)
            => new SqlDbRepository(connection);
    }
}
