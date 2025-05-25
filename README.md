# FlexDbLib

**FlexDbLib** is a lightweight, extensible, and SOLID-compliant .NET database abstraction library. It allows you to interact with multiple database engines (SQL Server, SQLite, etc.) using a consistent interface.

## ✅ Features

- 🔌 Plug-and-play repository pattern  
- 💡 Interface-driven architecture (SOLID principles)  
- 🛠 Easy to extend (MySQL, Oracle, PostgreSQL, etc.)  
- 🧪 Simple testability and mock support  
- 💾 Current support: SQL Server & SQLite  

---

## 🛠 Registering Providers

Before executing any database commands, register your database providers with the registry:

```csharp
DbProviderRegistry.Register("Sqlite", new SqliteProviderFactory());
DbProviderRegistry.Register("SqlServer", new SqlServerProviderFactory());


## ⚙️ Usage Example
```csharp
var factory = DbProviderRegistry.GetFactory("Sqlite");
using var connection = factory.CreateConnection();
connection.ConnectionString = "Data Source=mydb.db";
connection.Open();

using var command = connection.CreateCommand();
command.CommandText = "SELECT COUNT(*) FROM Users";
var result = command.ExecuteScalar();

if (result is int count)
{
    Console.WriteLine($"User count: {count}");
}
else
{
    Console.WriteLine("No result returned or null.");
}

## 🔍 ExecuteScalar Null Handling
public object? ExecuteScalar(string query, CommandType type, params DbParameter[] parameters)
{
    var command = CreateCommand(query, type, parameters);
    return command.ExecuteScalar();
}
