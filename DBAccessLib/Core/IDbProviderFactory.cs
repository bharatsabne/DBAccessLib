namespace DBAccessLib.Core
{
    public interface IDbProviderFactory
    {
        IDatabaseConnection CreateConnection(string connectionString);
        IRepository CreateRepository(IDatabaseConnection connection);
    }
}
