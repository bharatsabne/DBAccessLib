using System.Data.Common;

namespace DBAccessLib.Core
{
    public interface IDatabaseConnection
    {
        DbConnection Connection { get; }
        void Open();
        void Close();
        bool IsConnected();
    }
}
