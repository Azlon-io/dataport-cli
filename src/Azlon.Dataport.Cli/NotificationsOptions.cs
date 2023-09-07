using CommandLine;

[Verb("notifications", HelpText = "Subscribe to notifications.")]
public class NotificationsOptions : BaseOptions
{    
    [Option("dp-apitoken", Required = true, HelpText = "The dataport/iot connectionstring for sending and receiving DataPort messages.")]
    public string DataportApiToken { get; set; } = String.Empty;
    
    [Option("dp-apiaddress", Required = true, HelpText = "The dataport/iot connectionstring for sending and receiving DataPort messages.")]
    public string DataportApiAddress { get; set; } = String.Empty;

    [Option("dp-connectionstring", Required = true, HelpText = "The dataport/iot connectionstring for sending and receiving DataPort messages.")]
    public string DataportConnectionstring { get; set; } = String.Empty;
    
    [Option("blob-connectionstring", Required = true, HelpText = "The Azure Blob connectionstring for uploading DataPort files.")]
    public string AzureBlobConnectionstring { get; set; } = String.Empty;
    
    [Option("blob-container", Required = true, Default = "datafiles", HelpText = "The Azure Blob connectionstring for uploading DataPort files.")]
    public string AzureBlobContainer { get; set; } = String.Empty;
    
    [Option("blob-host", Required = true, Default = "dataports.blob.core.windows.net", HelpText = "The Azure Blob connectionstring for uploading DataPort files.")]
    public string AzureBlobHost { get; set; } = String.Empty;

}
