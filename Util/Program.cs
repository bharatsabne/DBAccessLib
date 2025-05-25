using DBAccessLib.Config;
using DBAccessLib.Core;
using DBAccessLib.Factories;
using System.Data;
using System.Data.Common;
using Util;

class Program
{
    static void Main(string[] args)
    {
        // 1. Register your DB providers here
        DbProviderRegistry.Register("Sqlite", new SqliteProviderFactory());
        DbProviderRegistry.Register("SqlServer", new SqlProviderFactory());
       

        // 2. Load DB configurations (this assumes you have a method to load configs)
        // For demo, we'll create a DbConfiguration instance manually:
        var sqliteConfig = new DbConfiguration
        {
            Name = "SqliteDb",
            Provider = "Sqlite",
            ConnectionString = "Data Source=C:\\Users\\JISL\\Downloads\\test.db;" // your sqlite connection string
        };

        var sqlConfig = new DbConfiguration
        {
            Name = "SqlDb",
            Provider = "SqlServer",
            ConnectionString = "Server=JITUND-0064\\SQLEXPRESS;" +
                               "Database=Readfines;" +
                               "User Id=sa;" +
                               "Password=Pass@1234;" +
                               "TrustServerCertificate=True;"
        };

        // 3. Create DB connection and repository
        var connection = sqliteConfig.CreateConnection();
        var repository = sqliteConfig.CreateRepository(connection);

        var sqlConnection = sqlConfig.CreateConnection();
        var sqlRepository = sqlConfig.CreateRepository(sqlConnection);

        var parameters = new[]
        {
            sqlRepository.CreateParameter("@Id", 1),
            sqlRepository.CreateParameter("@Name", "Alice"),
            sqlRepository.CreateParameter("@IsActive", true)
        };

        var result = sqlRepository.QueryToList<Book>("Test",
                     System.Data.CommandType.StoredProcedure, parameters);

        var result2 = repository.QueryToList<UserMaster>("SELECT * FROM User_Master");
    }
}
