using Amazon.Lambda.Core;
using ConfigCat.Client;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ConfigCatLambdaDemo;

public class Function
{
    private static readonly IConfigCatClient _configCatClient;

    static Function()
    {
#if DEBUG
        var sdkKey = "YOUR-CONFIGCAT-SDK-KEY-FOR-DEVELOPMENT";
        var cacheTimeToLive = TimeSpan.FromSeconds(5);
        var logger = new ConfigCatToLambdaLoggerAdapter(ConfigCat.Client.LogLevel.Info);
        // Use the Docker Redis cache setup in development
        var configCache = new RedisConfigCatCache("localhost:6379,password=myRedisPassword123,user=default", false);
#else
        var sdkKey = Environment.GetEnvironmentVariable("CONFIGCAT_SDK_KEY");
        if (string.IsNullOrWhiteSpace(sdkKey))
        {
            throw new InvalidOperationException("The CONFIGCAT_SDK_KEY environment variable is not set.");
        }

        var cacheTimeToLive = TimeSpan.FromSeconds(60);
        var logger = new ConfigCatToLambdaLoggerAdapter(ConfigCat.Client.LogLevel.Warning);

        var redisOssConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(redisOssConnectionString))
        {
            throw new InvalidOperationException("The REDIS_CONNECTION_STRING environment variable is not set.");
        }
        // Use ElastiCache when deployed to AWS
        var configCache = new RedisConfigCatCache(redisOssConnectionString, true);
#endif

        // Configure the ConfigCat client to use the Redis cache
        _configCatClient = ConfigCatClient.Get(sdkKey, options =>
        {
            options.ConfigCache = configCache;
            options.Logger = logger;
            options.PollingMode = PollingModes.LazyLoad(cacheTimeToLive);
        });
    }

    public static async Task<string> FunctionHandler(ILambdaContext context)
    {
        // Ensure the flag key here matches the one in your ConfigCat Dashboard
        var isFeatureEnabled = await _configCatClient.GetValueAsync("myFeatureFlag", false);

        if (isFeatureEnabled)
        {
            return "Result returned by new logic";
        }
        else
        {
            return "Result returned by old logic";
        }
    }
}
