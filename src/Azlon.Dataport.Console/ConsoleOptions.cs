using CommandLine;
using Microsoft.Extensions.Logging;

public class ConsoleOptions
{
    [Option('o', "out", Required = false, HelpText = "Folder path where the files should be saved to. [DP_OUT]", Default = "/dataport/downloads")]
    public string OutFolder { get; set; }

    [Option('l', "loglevel", Required = false, HelpText = "Log level: 'Critical', 'Debug', 'Error', 'Information', 'None', 'Trace', 'Warning'. [DP_LOGLEVEL]", Default = LogLevel.Information)]
    public LogLevel LogLevel { get; set; }

    [Option("environment", Required = false, HelpText = "Which environment to use (Live or Test). [DP_ENV]", Default = "Test")]
    public string Environment { get; set; }

    [Option("name", Required = false, HelpText = "Display name of the dataport. [DP_NAME]", Default = "DataPort")]
    public string Name { get; set; }

    [Option("apiAddress", Required = false, HelpText = "Url of the dataport Api, for example https://tstdataports.byzos.com/dataports or https://stgdataports.byzos.com/dataports. [DP_APIADDRESS]")]
    public string ApiAddress { get; set; }

    [Option('c', "connectionstring", Required = false, HelpText = "IOT hub connectionstring. [DP_CONNECTIONSTRING]")]
    public string Connectionstring { get; set; }

    [Option("sharedaccesskey", Required = false, HelpText = "IOT hub device shared access key. [DP_SHAREDACCESSKEY]")]
    public string SharedAccessKey { get; set; }

    [Option("deviceId", Required = false, HelpText = "IOT hub client device ID. [DP_DEVICEID]")]
    public string DeviceId { get; set; } // used by clients as request source prop

    [Option("AzureBlobContainer", Required = false, HelpText = "Azure blob storage container name. [DP_AZUREBLOBCONTAINER]")]
    public string AzureBlobContainer { get; set; }

    [Option("AzureBlobHost", Required = false, HelpText = "Azure blob storage hostname. [DP_AZUREBLOBHOST]")]
    public string AzureBlobHost { get; set; }

    [Option("ExtraFilesXpath", Required = false, HelpText = "The Xpath of file url(s) that also need to be downloaded. This Xpath is executed on the downloaded file on notifications. Multiple inputs allowed (space separated). [DP_EXTRAFILESXPATH]")]
    public IEnumerable<string> ExtraFilesXpath { get; set; }

}
