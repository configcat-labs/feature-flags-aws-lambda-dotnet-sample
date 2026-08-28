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
        var logLevel = ConfigCat.Client.LogLevel.Info;
        var cacheTimeToLive = TimeSpan.FromSeconds(5);
#else
        var sdkKey = "YOUR-CONFIGCAT-SDK-KEY-FOR-PRODUCTION";
        var logLevel = ConfigCat.Client.LogLevel.Warning;
        var cacheTimeToLive = TimeSpan.FromSeconds(60);
#endif

        // Credentials to connect to the local Redis server
        var redisConnectionString = "localhost:6379,password=myRedisPassword123,user=default";

        // Configure the ConfigCat client to use the Redis cache
        _configCatClient = ConfigCatClient.Get(sdkKey, options =>
        {
            options.ConfigCache = new RedisConfigCatCache(redisConnectionString);
            options.Logger = new ConfigCatToLambdaLoggerAdapter(logLevel);
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
