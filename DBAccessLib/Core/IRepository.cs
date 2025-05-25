using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessLib.Core
{
    public interface IRepository
    {
        IDataReader ExecuteReader(string query, CommandType type, params DbParameter[] parameters);
        object? ExecuteScalar(string query, CommandType type, params DbParameter[] parameters);
        IEnumerable<T> QueryToList<T>(string sql, CommandType commandType = CommandType.Text);
        IEnumerable<T> QueryToList<T>(string sql, CommandType commandType, params DbParameter[] parameters);
        int ExecuteNonQuery(string query, CommandType type, params DbParameter[] parameters);
        DataTable ExecuteDataTable(string query, CommandType type, params DbParameter[] parameters);
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
