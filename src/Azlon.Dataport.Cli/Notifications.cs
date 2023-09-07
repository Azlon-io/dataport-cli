using Microsoft.Extensions.Logging;
using Azlon.Dataport.Client;
using Azlon.Dataport.Model;
using Azlon.Dataport.Client.Configuration;

public class Notifications : IDisposable
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<Notifications> logger;
    private readonly NotificationsOptions options;
    private DataportClient? dataportClient;
    private bool disposedValue;

    public Notifications(ILoggerFactory loggerFactory, NotificationsOptions options)
    {
        this.loggerFactory = loggerFactory;
        this.logger = loggerFactory.CreateLogger<Notifications>();
        this.options = options;
    }

    public int SubscribeAndReturnExitCode()
    {
        try{
            Subscribe();
            return 0;
        }
        catch(Exception e)
        {
            logger.LogError(e.Message);
            return 1;
        }
    }

    public void Subscribe()
    {
        if (dataportClient == null)
        {
            // replace with connectionstring
            var dataportOptions = options.ToDataportOptions();
            dataportClient = new DataportClient(loggerFactory.CreateLogger<DataportClient>(), dataportOptions);
        }
        logger.LogInformation($"Subscribed");
        dataportClient.OnNotificationReceived += OnNotificationReceived;
        // dataportClient.OnRequestReceived += OnRequestReceived;
        dataportClient.StartReceivingMessages();
        // LogAction($"Connected to hub as {dataportName}");
    }

    private void OnNotificationReceived(object source, Notification message)
    {
        logger.LogInformation($"OnNotificationReceived");
        if (message != null)
        {
            logger.LogInformation($"OnNotificationReceived message");
            var fileName = Path.GetFileName(message.DataContainer.Uri.ToString());
            var fileUri = new Uri(fileName);
            var targetFolder = "./folderpath"; // from config!
            logger.LogInformation($"start download {fileName} {fileUri} {targetFolder}");
            // DownloadAndSaveMessage(message);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (dataportClient != null)
                {
                    dataportClient.StopReceivingMessages();
                    dataportClient = null;
                }
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Subscribe()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
