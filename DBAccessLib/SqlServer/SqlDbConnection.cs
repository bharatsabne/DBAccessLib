using DBAccessLib.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.SqlServer
{
    public class SqlDbConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public SqlDbConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
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
