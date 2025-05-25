// See https://aka.ms/new-console-template for more information
using DBAccessLib.Core;
using DBAccessLib.Sqlite;
using DBAccessLib.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

Console.WriteLine("Hello, World!");
//var config = new ConfigurationBuilder()
//            .SetBasePath(AppContext.BaseDirectory)
//            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//            .Build();

string? connStr;// = config.GetConnectionString("DBConnectionString");

connStr = "Server=JITUND-0064\\SQLEXPRESS;Database=Readfines;User Id=sa;Password=Pass@1234;TrustServerCertificate=True;";
// Instantiate the SQL Server connection and repository
if (connStr != null)
{
    var dbConn = new SqlDbConnection(connStr);
    var repository = new SqlDbRepository(dbConn);

    // Example usage: read from database
    try
    {
        var result = repository.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[Books]", System.Data.CommandType.Text);
        Console.WriteLine($"User count: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database error: " + ex.Message);
    }
}
else
{
    Console.WriteLine("NULL");
}
var connectionString = "Data Source=C:\\Users\\JISL\\Downloads\\JayFlexShipper.db";
IDatabaseConnection conn = new SqliteDbConnection(connectionString);
IRepository repo = new SqliteDbRepository(conn);
var result2 = repo.ExecuteScalar("SELECT COUNT(*) FROM Audit_Trail", System.Data.CommandType.Text);
Console.WriteLine($"Total Audit_Trail: {result2}");

