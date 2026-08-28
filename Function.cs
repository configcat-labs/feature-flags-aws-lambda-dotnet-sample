using Amazon.Lambda.Core;
using ConfigCat.Client;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ConfigCatLambdaDemo;

public class Function
{
    private static readonly IConfigCatClient _configCatClient;

    static Function()
    {
        var sdkKey = "YOUR-CONFIGCAT-SDK-KEY";

        // Credentials to connect to the local Redis server
        var redisConnectionString = "localhost:6379,password=myRedisPassword123,user=default";

        // Configure the ConfigCat client to use the Redis cache
        _configCatClient = ConfigCatClient.Get(sdkKey, options =>
        {
            options.ConfigCache = new RedisConfigCatCache(redisConnectionString);
            options.PollingMode = PollingModes.LazyLoad(cacheTimeToLive: TimeSpan.FromSeconds(60));
        });
    }

    public static async Task<string> FunctionHandler(ILambdaContext context)
    {
        // Ensure the flag key here matches the one in your ConfigCat Dashboard
        var isFeatureEnabled = await _configCatClient.GetValueAsync("myFeatureFlag", false);

        // Log the feature flag value
        context.Logger.LogInformation($"ConfigCat Flag value: {isFeatureEnabled}");

        if (isFeatureEnabled)
        {
            return "New Feature Logic Enabled";
        }

        return "Old Feature Active";
    }
}