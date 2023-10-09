using Microsoft.Extensions.Logging;
using CommandLine;
using System.Reflection;

public class Utilities
{
    public const string EnvironmentVariablesPrefix = "DP";

    public static async Task DownloadAndSaveToFileAsync(Uri url, string folderPath, string fileName)
    {
        Directory.CreateDirectory(folderPath);

        var content = await GetUrlContentAsync(url);
        if (content == null)
        {
            return;
        }

        await File.WriteAllBytesAsync(Path.Combine(folderPath, fileName), content);
    }

    private static async Task<byte[]?> GetUrlContentAsync(Uri url)
    {
        using var client = new HttpClient();
        using var result = await client.GetAsync(url);
        if (!result.IsSuccessStatusCode)
        {
            // TODO: Log error, don't interupt the app
        }
        return result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;
    }

    public static LogLevel GetLogLevel(string[] args)
    {
        var levels = Enum.GetNames<LogLevel>();
        foreach (var level in levels)
        {
            if (args.Contains($"-l={level}") || args.Contains($"--loglevel={level}") || Environment.GetEnvironmentVariable($"{EnvironmentVariablesPrefix}_LOGLEVEL") == level)
            {
                return Enum.Parse<LogLevel>(level);
            }
        }
        // default
        return LogLevel.Information;
    }

    public static string[] AppendEnvironmentVariables(string[] args, ILogger logger = null)
    {
        var options = GetOptions();

        var newArgs = new List<string>(args);
        foreach (var unusedOption in FilterOptions(args, options))
        {
            // repalced this 'Assembly.GetExecutingAssembly().GetName().Name.ToUpperInvariant()' with a static prefix of '{EnvironmentVariablesPrefix}_'
            // ignore verbs since this solution has only 1
            var value = Environment.GetEnvironmentVariable($"{EnvironmentVariablesPrefix}_{unusedOption.LongName.ToUpperInvariant()}");

            // try reading a docker secret, ignore errors
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.LogDebug($"Trying to read docker secret for {unusedOption.LongName} at /run/secrets/{EnvironmentVariablesPrefix}_{unusedOption.LongName.ToUpperInvariant()}");
                try
                {
                    value = File.ReadAllText($"/run/secrets/{EnvironmentVariablesPrefix}_{unusedOption.LongName.ToUpperInvariant()}");
                }
                catch { }
            }

            if (value != null)
            {
                newArgs.Add($"--{unusedOption.LongName}={value}");
            }
        }
        return newArgs.ToArray();
    }

    private static OptionAttribute[] GetOptions()
    {
        return typeof(ConsoleOptions).GetProperties()
            .Select(property => property.GetCustomAttributes<OptionAttribute>().FirstOrDefault())
            .Where(option => option != null).ToArray();
    }

    private static OptionAttribute[] FilterOptions(string[] args, OptionAttribute[] options)
    {
        var usedLongNames = new HashSet<string>();
        var usedShortNames = new HashSet<string>();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                var longName = arg.Substring(2);
                if (longName.Contains('='))
                {
                    longName = longName.Substring(0, longName.IndexOf('='));
                }
                usedLongNames.Add(longName);
            }
            else if (arg.StartsWith("-"))
            {
                var shortName = arg.Substring(1);
                if (shortName.Contains('='))
                {
                    shortName = shortName.Substring(0, shortName.IndexOf('='));
                }
                usedShortNames.Add(shortName);
            }
        }

        return options.Where(option => !usedLongNames.Contains(option.LongName) && !usedShortNames.Contains(option.ShortName)).ToArray();
    }

}