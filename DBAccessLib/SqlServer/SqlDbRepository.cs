using DBAccessLib.Core;
using DBAccessLib.Core.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBAccessLib.SqlServer
{
    public class SqlDbRepository : IRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private SqlTransaction? _transaction;

        public SqlDbRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

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
        public IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text)
        {
            _dbConnection.Open();

            using var command = ((SqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;

            using var reader = command.ExecuteReader();

            var results = new List<T>();
            var props = typeof(T).GetProperties();

            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in props)
                {
                    if (!reader.HasColumn(prop.Name) || reader[prop.Name] is DBNull)
                        continue;

                    var dbValue = reader[prop.Name];
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    var safeValue = Convert.ChangeType(dbValue, targetType);
                    prop.SetValue(obj, safeValue);
                }
                results.Add(obj);
            }

            _dbConnection.Close();
            return results;

        }
        public IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters)
        {
            _dbConnection.Open();

            using var command = ((SqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;

            if (parameters != null && parameters.Length > 0)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            using var reader = command.ExecuteReader();

            var results = new List<T>();
            var props = typeof(T).GetProperties();

            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in props)
                {
                    if (!reader.HasColumn(prop.Name) || reader[prop.Name] is DBNull)
                        continue;

                    var dbValue = reader[prop.Name];
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    var safeValue = Convert.ChangeType(dbValue, targetType);
                    prop.SetValue(obj, safeValue);
                }
                results.Add(obj);
            }

            _dbConnection.Close();
            return results;

        }
        public int ExecuteNonQuery(string query, CommandType type, params DbParameter[] parameters)
        {
            var command = CreateCommand(query, type, parameters);
            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteDataTable(string query, CommandType type, params DbParameter[] parameters)
        {
            using var adapter = new SqlDataAdapter((SqlCommand)CreateCommand(query, type, parameters));
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void BeginTransaction()
        {
            _dbConnection.Open();
            _transaction = ((SqlConnection)_dbConnection.Connection).BeginTransaction();
        }

        public void Commit() => _transaction?.Commit();
        public void Rollback() => _transaction?.Rollback();

        private SqlCommand CreateCommand(string query, CommandType type, DbParameter[] parameters)
        {
            _dbConnection.Open();
            var command = ((SqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = query;
            command.CommandType = type;
            command.Transaction = _transaction;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            return command;
        }
    }
}
