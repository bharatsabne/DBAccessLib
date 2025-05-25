using DBAccessLib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Sqlite
{
    public class SqliteDbRepository : IRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private DbTransaction? _transaction;

        public SqliteDbRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void BeginTransaction()
        {
            _dbConnection.Open();
            _transaction = _dbConnection.Connection.BeginTransaction();
        }

        public void Commit() => _transaction?.Commit();
        public void Rollback() => _transaction?.Rollback();

        public IDataReader ExecuteReader(string query, CommandType type, params DbParameter[] parameters)
        {
            var command = CreateCommand(query, type, parameters);
            return command.ExecuteReader();
        }

        public object? ExecuteScalar(string query, CommandType type, params DbParameter[] parameters)
        {
            var command = CreateCommand(query, type, parameters);
            return command.ExecuteScalar();
        }

        public int ExecuteNonQuery(string query, CommandType type, params DbParameter[] parameters)
        {
            var command = CreateCommand(query, type, parameters);
            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteDataTable(string query, CommandType type, params DbParameter[] parameters)
        {
            using var adapter = new SQLiteDataAdapter((SQLiteCommand)CreateCommand(query, type, parameters));
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        private DbCommand CreateCommand(string query, CommandType type, DbParameter[] parameters)
        {
            _dbConnection.Open();
            var command = _dbConnection.Connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = type;
            command.Transaction = _transaction;
            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            return command;
        }
    }
}
