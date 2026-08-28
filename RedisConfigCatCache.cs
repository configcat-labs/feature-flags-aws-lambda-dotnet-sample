using Amazon.Lambda.Core;
using ConfigCat.Client;
using StackExchange.Redis;

namespace ConfigCatLambdaDemo;

public class RedisConfigCatCache : IConfigCatCache
{
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public RedisConfigCatCache(string connectionString)
    {

        var options = ConfigurationOptions.Parse(connectionString);
        options.AbortOnConnectFail = false;
        options.ConnectTimeout = 2000;
        options.SyncTimeout = 2000;

        _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
    }

    private IDatabase Database => _connection.Value.GetDatabase();

    public async ValueTask<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return await Database.StringGetAsync(key);
        }
        catch (RedisException e)
        {
            // Log get warning 
            LambdaLogger.Log($"Error getting redis cache value: {e}");
            return null;
        }
    }

    public async ValueTask SetAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        try
        {
            await Database.StringSetAsync(key, value);
        }
        catch (RedisException e)
        {
            // Log set warning
            LambdaLogger.Log($"Error setting redis cache value: {e}");
        }
    }
}