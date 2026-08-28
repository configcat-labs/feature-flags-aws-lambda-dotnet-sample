using System.Globalization;
using Amazon.Lambda.Core;
using ConfigCat.Client;

namespace ConfigCatLambdaDemo;

using LogLevel = ConfigCat.Client.LogLevel;

public class ConfigCatToLambdaLoggerAdapter : IConfigCatLogger
{
    private volatile LogLevel _logLevel;

    public ConfigCatToLambdaLoggerAdapter(LogLevel logLevel = LogLevel.Warning)
    {
        _logLevel = logLevel;
    }

    public LogLevel LogLevel { get => _logLevel; set => _logLevel = value; }

    public void Log(LogLevel level, LogEventId eventId, ref FormattableLogMessage message, Exception? exception = null)
    {
        Amazon.Lambda.Core.LogLevel lambdaLogLevel;

        switch (level)
        {
            case LogLevel.Debug: lambdaLogLevel = Amazon.Lambda.Core.LogLevel.Debug; break;
            case LogLevel.Info: lambdaLogLevel = Amazon.Lambda.Core.LogLevel.Information; break;
            case LogLevel.Warning: lambdaLogLevel = Amazon.Lambda.Core.LogLevel.Warning; break;
            case LogLevel.Error: lambdaLogLevel = Amazon.Lambda.Core.LogLevel.Error; break;
            default: return;
        }

        var messageFormat = $"ConfigCat[{eventId.Id.ToString(CultureInfo.InvariantCulture)}] {message.Format}";
        LambdaLogger.Log(lambdaLogLevel, exception, messageFormat, message.ArgValues ?? []);
    }
}
