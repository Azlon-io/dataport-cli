using CommandLine;
using Microsoft.Extensions.Logging;

public abstract class BaseOptions
{    
    [Option('l', "loglevel", Required = false, HelpText = "The loglevel", Default = Utilities.DefaultLogLevel)]
    public LogLevel LogLevel { get; set; }
}
