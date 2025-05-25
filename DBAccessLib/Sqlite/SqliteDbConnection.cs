using DBAccessLib.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Sqlite    
{
    public class SqliteDbConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private readonly SQLiteConnection _connection;

        public SqliteDbConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(_connectionString);
        }

        public DbConnection Connection => _connection;

        public void Open()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }

        public void Close()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        public bool IsConnected()
        {
            try
            {
                Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
