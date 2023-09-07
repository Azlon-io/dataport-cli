using System;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Logging;

public static class Utilities
{
    public const LogLevel DefaultLogLevel = LogLevel.Debug;
    
    public static LogLevel GetLogLevel(string[] args)
    {
        var levels = Enum.GetNames<LogLevel>();
        foreach(var level in levels)
        {
            if (args.Contains($"-l={level}") || args.Contains($"--loglevel={level}") || Environment.GetEnvironmentVariable("DP_LOGLEVEL") == level)
            {
                return Enum.Parse<LogLevel>(level);
            }
        }
        // default
        return DefaultLogLevel;
    }

    /// <summary>
    /// Extends command line arguments with environment variable inputs.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static string[] AppendEnvironmentVariables(string[] args, ILogger logger)
    {
        var options = GetOptions();
        
        var newArgs = new List<string>(args);
        foreach (var unusedOption in FilterOptions(args, options))
        {
            // repalced this 'Assembly.GetExecutingAssembly().GetName().Name.ToUpperInvariant()' with a static prefix of 'DP_'
            // ignore verbs since this solution has only 1
            var value = Environment.GetEnvironmentVariable($"DP_{unusedOption.LongName.ToUpperInvariant()}");
            
            // try reading a docker secret, ignore errors
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.LogDebug($"Trying to read docker secret for {unusedOption.LongName} at /run/secrets/DP_{unusedOption.LongName.ToUpperInvariant()}");
                try
                {
                    value = File.ReadAllText($"/run/secrets/DP_{unusedOption.LongName.ToUpperInvariant()}");
                }
                catch{}
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
        // works with 1 verb only, requires a refactor for multi-verb, using reflection to loop through verbs
        return typeof(NotificationsOptions).GetProperties()
            .Select(property => property.GetCustomAttributes<OptionAttribute>().FirstOrDefault())
            .Where(option => option != null).Cast<OptionAttribute>().ToArray();
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
