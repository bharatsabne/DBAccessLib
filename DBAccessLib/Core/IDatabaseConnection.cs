using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
