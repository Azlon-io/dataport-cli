using Microsoft.Extensions.Logging;
using CommandLine;
using System.Reflection;
using Azlon.Dataport.Client;
public static class Extensions
{
    public static DataportOptions ToDataportOptions(this ConsoleOptions options)
    {
        return new DataportOptions()
        {
            AzureBlobConnectionstring = options.AzureBlobConnectionstring,
            AzureBlobContainer = options.AzureBlobContainer,
            AzureBlobHost = options.AzureBlobHost,
            DataportApiAddress = options.DataportApiAddress,
            DataportApiToken = options.DataportApiToken,
            DataportConnectionstring = options.DataportConnectionstring,
            DataportDeviceId = options.DataportDeviceId,
        };
    }
}
