using DBAccessLib.Core;
using DBAccessLib.Core.Attributes;
using DBAccessLib.Core.Extensions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DBAccessLib.MySql
{
    public class MySqlDbRepository : IRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private MySqlTransaction? _transaction;

        public MySqlDbRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public DbParameter CreateParameter(string name, object value)
        {
            var param = new MySqlParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value
            };
            return param;
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

        public IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters)
        {
            _dbConnection.Open();

            using var command = ((MySqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            using var reader = command.ExecuteReader();
            var results = new List<T>();
            var props = typeof(T).GetProperties();

            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in props)
                {
                    var columnAttr = prop.GetCustomAttribute<ColumnNameAttribute>();
                    var columnName = columnAttr?.Name ?? prop.Name;

                    if (!reader.HasColumn(columnName) || reader[columnName] is DBNull)
                        continue;

                    var dbValue = reader[columnName];
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
            using var adapter = new MySqlDataAdapter((MySqlCommand)CreateCommand(query, type, parameters));
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void BeginTransaction()
        {
            _dbConnection.Open();
            _transaction = ((MySqlConnection)_dbConnection.Connection).BeginTransaction();
        }

        public void Commit() => _transaction?.Commit();
        public void Rollback() => _transaction?.Rollback();

        private MySqlCommand CreateCommand(string query, CommandType type, DbParameter[] parameters)
        {
            _dbConnection.Open();
            var command = ((MySqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = query;
            command.CommandType = type;
            command.Transaction = _transaction;

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            return command;
        }

        public int Insert<T>(T entity)
        {
            var type = typeof(T);
            var tableName = type.Name;
            var props = type.GetProperties();

            var columns = string.Join(", ", props.Select(p => $"`{p.Name}`"));
            var values = string.Join(", ", props.Select(p => "@" + p.Name));
            var sql = $"INSERT INTO `{tableName}` ({columns}) VALUES ({values})";

            using var command = ((MySqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var prop in props)
            {
                var param = command.CreateParameter();
                param.ParameterName = "@" + prop.Name;
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            _dbConnection.Open();
            var result = command.ExecuteNonQuery();
            _dbConnection.Close();

            return result;
        }

        public int Update<T>(T entity, string[] keyColumns)
        {
            var type = typeof(T);
            var tableName = type.Name;
            var props = type.GetProperties();

            var keyProps = props.Where(p => keyColumns.Contains(p.Name)).ToList();
            var nonKeyProps = props.Except(keyProps).ToList();

            if (!keyProps.Any())
                throw new ArgumentException("At least one key column must be specified.");

            var setClause = string.Join(", ", nonKeyProps.Select(p => $"`{p.Name}` = @{p.Name}"));
            var whereClause = string.Join(" AND ", keyProps.Select(p => $"`{p.Name}` = @{p.Name}"));

            var sql = $"UPDATE `{tableName}` SET {setClause} WHERE {whereClause}";

            using var command = ((MySqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var prop in props)
            {
                var param = command.CreateParameter();
                param.ParameterName = "@" + prop.Name;
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            _dbConnection.Open();
            var result = command.ExecuteNonQuery();
            _dbConnection.Close();

            return result;
        }

        public int Delete<T>(T entity, string[] keyColumns)
        {
            var type = typeof(T);
            var tableName = type.Name;
            var props = type.GetProperties();

            var keyProps = props.Where(p => keyColumns.Contains(p.Name)).ToList();
            if (!keyProps.Any())
                throw new ArgumentException("At least one key column must be specified.");

            var whereClause = string.Join(" AND ", keyProps.Select(p => $"`{p.Name}` = @{p.Name}"));
            var sql = $"DELETE FROM `{tableName}` WHERE {whereClause}";

            using var command = ((MySqlConnection)_dbConnection.Connection).CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var prop in keyProps)
            {
                var param = command.CreateParameter();
                param.ParameterName = "@" + prop.Name;
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            _dbConnection.Open();
            var result = command.ExecuteNonQuery();
            _dbConnection.Close();

            return result;
        }

        public IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text)
        {
            _dbConnection.Open();

            using var command = _dbConnection.Connection.CreateCommand();
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
                    var columnAttr = prop.GetCustomAttribute<ColumnNameAttribute>();
                    var columnName = columnAttr?.Name ?? prop.Name;

                    if (!reader.HasColumn(columnName) || reader[columnName] is DBNull)
                        continue;

                    var dbValue = reader[columnName];
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var safeValue = Convert.ChangeType(dbValue, targetType);
                    prop.SetValue(obj, safeValue);
                }
                results.Add(obj);
            }

            _dbConnection.Close();
            return results;
        }
    }
}
