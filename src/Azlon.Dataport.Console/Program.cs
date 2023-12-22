using CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Azlon.Dataport.Client;
using Serilog;
using Microsoft.Extensions.Configuration;

public class Program
{
    static CancellationTokenSource cts = new CancellationTokenSource();
    static void Main(string[] args)
    {     
        // init logger
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(Utilities.GetLogLevel(args));
        });
        Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger<Program>();
        logger.LogDebug("Logger Factory initialized.");

#if DEBUG
        Console.WriteLine("Running in Debug mode.");
        DotNetEnv.Env.Load();
        Console.WriteLine("DotNetEnv Loaded.");
#endif

        // fetch arguments & env vars
        args = Utilities.AppendEnvironmentVariables(args, logger);
        var consoleArguments = Parser.Default.ParseArguments<ConsoleOptions>(args);
        if (consoleArguments.Errors.Count() > 0)
        {
            return;
        }

        // configure shutdown
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;  // Prevent the immediate exit.
            cts.Cancel();
        };
        AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
        {
            // Handle SIGTERM for graceful shutdown in containers
            logger.LogDebug("Exiting.");
            cts.Cancel();
        };

        // register stuff

        var builder = new ConfigurationBuilder();
        builder.AddEnvironmentVariables();
        var configuration = builder.Build();

        var serilogLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole().SetMinimumLevel(Utilities.GetLogLevel(args));

            builder.AddSerilog(serilogLogger);
        });
        var consoleOptions = consoleArguments.Value;
        var dataportOptions = consoleOptions.ToDataportOptions();
        serviceCollection.AddSingleton(consoleOptions);
        serviceCollection.AddSingleton<IDataportOptions>(dataportOptions);
        serviceCollection.AddSingleton<Notifications>();
        serviceCollection.AddSingleton<IDataportService, DataportService>();
        serviceCollection.AddSingleton<DataportClient>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // execute stuff
        var notifications = serviceProvider.GetRequiredService<Notifications>();
        var client = serviceProvider.GetRequiredService<DataportClient>();
        client.OnNotificationReceived += notifications.OnNotificationReceived;
        client.StartReceivingMessages(); // request & response messages are not supported

        // keep running
        while (!cts.Token.IsCancellationRequested)
        {
#if DEBUG
            Console.Write(".");
#endif
            // Keeps the console running
            Thread.Sleep(1000);
        }

        // dispose stuff
        loggerFactory.Dispose();
    }
}
