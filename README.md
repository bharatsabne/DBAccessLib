# DBAccessSharp

**DBAccessSharp** is a lightweight, extensible, and SOLID-compliant .NET database abstraction library. It allows you to interact with multiple database engines (e.g., SQL Server, SQLite, MySQL) using a clean and consistent interface.


---

## 🎯 Purpose

> This library is designed as a **study project** to demonstrate the use of **SOLID principles**, interface-based architecture, and clean code practices in real-world .NET applications.  
> Even though most applications use a single database, **DBAccessSharp** enables support for multiple providers and showcases how to write **clean, testable, and extensible** data access code.

---
## ✅ Features

- 🔌 Plug-and-play repository pattern  
- 💡 Interface-driven architecture (adheres to SOLID principles)  
- 🧱 Easily extensible (e.g., MySQL, PostgreSQL, Oracle, etc.)  
- 🧪 Simple to unit test and mock  
- 💾 Built-in support for: **SQL Server**, **SQLite**, and **MySQL**

---

## 🛠 Registering Providers

Before using the library, register supported database providers:

```csharp
DbProviderRegistry.Register("Sqlite", new SqliteProviderFactory());
DbProviderRegistry.Register("SqlServer", new SqlProviderFactory());
DbProviderRegistry.Register("MySql", new MySqlProviderFactory());
```

## ⚙️ Usage Example
```csharp
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

        // 2. Define DB configurations manually or load from a config file
        var sqliteConfig = new DbConfiguration
        {
            Name = "SqliteDb",
            Provider = "Sqlite",
            ConnectionString = "Data Source=C:\\Path\\To\\Your\\test.db;"
        };

        var sqlConfig = new DbConfiguration
        {
            Name = "SqlDb",
            Provider = "SqlServer",
            ConnectionString = "Server=SERVERNAME;Database=DatabaseName;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
        };

        var mysqlConfig = new DbConfiguration
        {
            Name = "MYSQL",
            Provider = "MySql",
            ConnectionString = "Server=localhost;Port=3306;Database=readfine_readfinesbook;User Id=root;Password=;"
        };

        // 3. Create connections and repositories
        var sqliteRepo = sqliteConfig.CreateRepository(sqliteConfig.CreateConnection());
        var sqlRepo = sqlConfig.CreateRepository(sqlConfig.CreateConnection());
        var mysqlRepo = mysqlConfig.CreateRepository(mysqlConfig.CreateConnection());

        // SQL Server: Call stored procedure with parameters
        var parameters = new[]
        {
            sqlRepo.CreateParameter("@Id", 1),
            sqlRepo.CreateParameter("@Name", "Alice"),
            sqlRepo.CreateParameter("@IsActive", true)
        };

        var resultSQL = sqlRepo.QueryToList<Book>("Test", CommandType.StoredProcedure, parameters);
        Console.WriteLine($"SQL Server Result Count: {resultSQL.Count()}");
        foreach (var book in resultSQL)
        {
            Console.WriteLine($"Book ID: {book.Id}, Title: {book.MyBookNameInEnglish}, Author: {book.AuthorId}");
        }

        // SQLite: Simple select query
        var resultSQLite = sqliteRepo.QueryToList<UserMaster>("SELECT * FROM User_Master");
        Console.WriteLine($"SQLite Result Count: {resultSQLite.Count()}");
        foreach (var user in resultSQLite)
        {
            Console.WriteLine($"User ID: {user.User_Code}, Name: {user.UserName}, IsActive: {user.IsActive}");
        }

        // MySQL: DataTable and entity mapping
        var resultMySQL = mysqlRepo.ExecuteDataTable("SELECT * FROM dbobookdetails", CommandType.Text);
        Console.WriteLine($"MySQL Result Count: {resultMySQL.Rows.Count}");
        foreach (DataRow row in resultMySQL.Rows)
        {
            Console.WriteLine($"Book ID: {row["ID"]}, Title: {row["BookName"]}, Author: {row["Author"]}");
        }

        var mysqlLanguages = mysqlRepo.QueryToList<Language>("SELECT * FROM dbolanguage");
        Console.WriteLine($"MySQL Language Result Count: {mysqlLanguages.Count()}");
        foreach (var language in mysqlLanguages)
        {
            Console.WriteLine($"Language ID: {language.Id}, Name: {language.LanguageName}");
        }
    }
}

```

## 🔍 ExecuteScalar Null Handling
```csharp
public object? ExecuteScalar(string query, CommandType type, params DbParameter[] parameters)
{
    var command = CreateCommand(query, type, parameters);
    return command.ExecuteScalar();
}
```

## 🧩 Model Examples

```csharp
public class Book
{
    public int Id { get; set; }
    [ColumnName("BookName")] //using DB Mappers
    public string MyBookNameInEnglish { get; set; }
    public int AuthorId { get; set; }
}

public class UserMaster
{
    public int User_Code { get; set; }
    public string UserName { get; set; }
    public bool IsActive { get; set; }
}

public class Language
{
    public int Id { get; set; }
    public string LanguageName { get; set; }
}
```
## 🧪 Testing Tip
You can mock IDbRepository for unit testing. Example:
```csharp
var mockRepo = new Mock<IDbRepository>();
mockRepo.Setup(r => r.QueryToList<UserMaster>("SELECT * FROM User_Master"))
        .Returns(new List<UserMaster> { new UserMaster { User_Code = 1, UserName = "Test", IsActive = true } });

```

## 🔮 Future Enhancements
 - Add PostgreSQL and Oracle support
 - Add DbConfigurationLoader for reading config from files
 - Add logging support (ILogger)
 - Add unit test projects and mocking samples

## 📚 Learning Objectives
 - Apply SOLID principles in .NET applications
 - Learn Repository pattern with interface-based design
 - Practice writing clean, extensible, and testable code
 - Understand how to support multiple DB providers with minimal coupling
