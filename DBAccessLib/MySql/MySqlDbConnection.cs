using DBAccessLib.Core;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace DBAccessLib.MySql
{
    public class MySqlDbConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public MySqlDbConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new MySqlConnection(_connectionString);
        }

        public DbConnection Connection => _connection;

        public void Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
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
