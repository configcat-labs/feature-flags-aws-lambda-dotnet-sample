using ConfigCat.Client;
using StackExchange.Redis;

namespace ConfigCatLambdaDemo;

public class ElastiCacheConfigCatCache : IConfigCatCache
{
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public ElastiCacheConfigCatCache(string connectionString)
    {
        var options = ConfigurationOptions.Parse(connectionString);

        options.AbortOnConnectFail = false;
        options.ConnectTimeout = 2000;
        options.SyncTimeout = 2000;
        options.Ssl = true;

        _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
    }

    private IDatabase Database => _connection.Value.GetDatabase();

    public async ValueTask<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await Database.StringGetAsync(key);
    }

    public async ValueTask SetAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        await Database.StringSetAsync(key, value);
    }
}
