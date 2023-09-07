using CommandLine;
using Microsoft.Extensions.Logging;

public class Program
{
    static CancellationTokenSource cts = new CancellationTokenSource();

    static void Main(string[] args)
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(Utilities.GetLogLevel(args));
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();
        
        args = Utilities.AppendEnvironmentVariables(args, logger);
        Parser.Default.ParseArguments<NotificationsOptions, object>(args) // <NotificationsOptions, SubscribeOptions>
            .WithParsed<NotificationsOptions>(options => {
                using(var notifications = new Notifications(loggerFactory, options))
                {
                    notifications.Subscribe();
                }
            })
            //.WithParsed<SubscribeOptions>(options => {
            //    using (var subscribe = new Subscribe(loggerFactory.CreateLogger<Subscribe>(), options))
            //    {
            //        subscribe.Run();
            //    }
            //})
            .WithNotParsed(errors => {
                return;
            });

        Console.CancelKeyPress += (sender, e) => {
            logger.LogDebug("CancelKeyPress");
            e.Cancel = true;  // Prevent the immediate exit.
            cts.Cancel();
        };
        
        // Handle SIGTERM for graceful shutdown in containers
        AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
            logger.LogDebug("ProcessExit");
            cts.Cancel();
        };

        while (!cts.Token.IsCancellationRequested)
        {
            // Keeps the console running
            Thread.Sleep(100);
        }
    
        loggerFactory.Dispose();
    }

}