namespace DBAccessLib.Core
{
    public static class DbProviderRegistry
    {
        private static readonly Dictionary<string, IDbProviderFactory> _factories = new();

        public static void Register(string providerName, IDbProviderFactory factory)
        {
            _factories[providerName] = factory;
        }

        public static IDbProviderFactory GetFactory(string providerName)
        {
            if (_factories.TryGetValue(providerName, out var factory))
                return factory;

            throw new NotSupportedException($"No factory registered for provider: {providerName}");
        }
    }
}
