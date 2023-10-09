using CommandLine;
using Microsoft.Extensions.Logging;

public class ConsoleOptions
{
    [Option('o', "out", Required = false, HelpText = "Folder path where the files should be saved to. [DP_OUT]", Default = "/dataport/downloads")]
    public string OutFolder { get; set; }

    [Option('l', "loglevel", Required = false, HelpText = "Log level: 'Critical', 'Debug', 'Error', 'Information', 'None', 'Trace', 'Warning'. [DP_LOGLEVEL]", Default = LogLevel.Information)]
    public LogLevel LogLevel { get; set; }

    [Option("apiToken", Required = false, HelpText = "JWT Token of the dataport Api. [DP_APITOKEN]", Default = "")]
    public string DataportApiToken { get; set; } // not implemented yet on the api

    [Option("apiAddress", Required = false, HelpText = "Url of the dataport Api, for example https://tstdataports.byzos.com/dataports or https://stgdataports.byzos.com/dataports. [DP_APIADDRESS]", Default = "http://localhost:8001/dataports")]
    public string DataportApiAddress { get; set; }

    [Option('c', "connectionstring", Required = false, HelpText = "IOT hub connectionstring. [DP_CONNECTIONSTRING]")]
    public string DataportConnectionstring { get; set; }

    [Option("deviceId", Required = false, HelpText = "IOT hub client device ID. [DP_DEVICEID]")]
    public string DataportDeviceId { get; set; } // used by clients as request source prop

    [Option("AzureBlobConnectionstring", Required = false, HelpText = "Azure blob storage connectionstring. [DP_AZUREBLOBCONNECTIONSTRING]")]
    public string AzureBlobConnectionstring { get; set; }

    [Option("AzureBlobContainer", Required = false, HelpText = "Azure blob storage container name. [DP_AZUREBLOBCONTAINER]", Default = "datafiles")]
    public string AzureBlobContainer { get; set; }

    [Option("AzureBlobHost", Required = false, HelpText = "Azure blob storage hostname. [DP_AZUREBLOBHOST]", Default = "dataports.blob.core.windows.net")]
    public string AzureBlobHost { get; set; }

    [Option("ExtraFilesXpath", Required = false, HelpText = "The Xpath of file url(s) that also need to be downloaded. This Xpath is executed on the downloaded file on notifications. Multiple inputs allowed (space separated). [DP_EXTRAFILESXPATH]")]
    public IEnumerable<string> ExtraFilesXpath { get; set; }

}
