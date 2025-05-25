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
        DbProviderRegistry.Register("MySql", new MySqlProviderFactory()); 


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

        var mysql = new DbConfiguration
        {
            Name = "MYSQL",
            Provider = "MySql",
            ConnectionString = "Server=localhost;Port=3306;Database=readfine_readfinesbook;User Id=root;Password=;"
        };

        // 3. Create DB connection and repository
        var connection = sqliteConfig.CreateConnection();
        var repository = sqliteConfig.CreateRepository(connection);

        var sqlConnection = sqlConfig.CreateConnection();
        var sqlRepository = sqlConfig.CreateRepository(sqlConnection);

        var mysqlConnection = mysql.CreateConnection();
        var mysqlRepository = mysql.CreateRepository(mysqlConnection);

        var parameters = new[]
        {
            sqlRepository.CreateParameter("@Id", 1),
            sqlRepository.CreateParameter("@Name", "Alice"),
            sqlRepository.CreateParameter("@IsActive", true)
        };

        var resultSQL = sqlRepository.QueryToList<Book>("Test",
                     System.Data.CommandType.StoredProcedure, parameters);

        Console.WriteLine($"SQL Server Result Count: {resultSQL.Count()}");
        foreach (var book in resultSQL)
        {
            Console.WriteLine($"Book ID: {book.Id}, Title: {book.MyBookNameInEnglish}, Author: {book.AuthorId}");
        }
        var resultSQLite = repository.QueryToList<UserMaster>("SELECT * FROM User_Master");
        Console.WriteLine($"SQLite Result Count: {resultSQLite.Count()}");
        foreach (var user in resultSQLite)
        {
            Console.WriteLine($"User ID: {user.User_Code}, Name: {user.UserName}, IsActive: {user.IsActive}");
        }
        var resultMySQl = mysqlRepository.ExecuteDataTable("SELECT * FROM dbobookdetails",CommandType.Text);
        Console.WriteLine($"MySQL Result Count: {resultMySQl.Rows.Count}");
        foreach (DataRow row in resultMySQl.Rows)
        {
            Console.WriteLine($"Book ID: {row["ID"]}, Title: {row["BookName"]}, Author: {row["Author"]}");
        }
        var mysqlResult = mysqlRepository.QueryToList<Language>("Select * from dbolanguage");
        Console.WriteLine($"MySQL Language Result Count: {mysqlResult.Count()}");
        foreach (var language in mysqlResult)
        {
            Console.WriteLine($"Language ID: {language.Id}, Name: {language.LanguageName}");
        }
    }
}
