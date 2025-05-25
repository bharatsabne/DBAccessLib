using DBAccessLib.Core;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Config
{
    public class DbConfiguration
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string ConnectionString { get; set; }

        public IDatabaseConnection CreateConnection()
        {
            var factory = DbProviderRegistry.GetFactory(Provider);
            return factory.CreateConnection(ConnectionString);
        }

        public IRepository CreateRepository(IDatabaseConnection connection)
        {
            var factory = DbProviderRegistry.GetFactory(Provider);
            return factory.CreateRepository(connection);
        }
    }
}
