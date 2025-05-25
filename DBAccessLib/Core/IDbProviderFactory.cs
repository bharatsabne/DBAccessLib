using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Core
{
    public interface IDbProviderFactory
    {
        IDatabaseConnection CreateConnection(string connectionString);
        IRepository CreateRepository(IDatabaseConnection connection);
    }
}
