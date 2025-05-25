using DBAccessLib.Core;
using DBAccessLib.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Config
{
    public static class DbProviderRegistration
    {
        public static void RegisterAll()
        {
            DbProviderRegistry.Register("Sqlite", new SqliteProviderFactory());
            DbProviderRegistry.Register("SqlServer", new SqlProviderFactory());
            DbProviderRegistry.Register("MySql", new MySqlProviderFactory());
        }
        public static void RegisterSqlite()
        {
            DbProviderRegistry.Register("Sqlite", new SqliteProviderFactory());
        }

        public static void RegisterSqlServer()
        {
            DbProviderRegistry.Register("SqlServer", new SqlProviderFactory());
        }

        public static void RegisterMySql()
        {
            DbProviderRegistry.Register("MySql", new MySqlProviderFactory());
        }
    }
}
