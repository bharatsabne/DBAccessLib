using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DBAccessLib.Config
{
    public static class DbConfiguration
    {
        public static string GetConnectionString(string name = "DBConnectionString")
        {
            return "Server=JITUND-0064\\SQLEXPRESS;Database=Readfines;User Id=sa;Password=Pass@1234;TrustServerCertificate=True;";
        }
    }
}
