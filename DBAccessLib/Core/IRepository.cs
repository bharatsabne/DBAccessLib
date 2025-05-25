using System.Data;
using System.Data.Common;

namespace DBAccessLib.Core
{
    public interface IRepository
    {
        DbParameter CreateParameter(string name, object value);
        IDataReader ExecuteReader(string query, CommandType type, params DbParameter[] parameters);
        object? ExecuteScalar(string query, CommandType type, params DbParameter[] parameters);
        IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text);
        IEnumerable<T> QueryToList<T>(string sql, CommandType commandType, params DbParameter[] parameters);
        int ExecuteNonQuery(string query, CommandType type, params DbParameter[] parameters);
        DataTable ExecuteDataTable(string query, CommandType type, params DbParameter[] parameters);
        void BeginTransaction();
        void Commit();
        void Rollback();

        int Insert<T>(T entity);
        int Update<T>(T entity, string[] keyColumns);
        int Delete<T>(T entity, string[] keyColumns);
    }
}
