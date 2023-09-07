
using Azlon.Dataport.Client;
public static class Extensions
{

    public static DataportOptions ToDataportOptions(this NotificationsOptions options)
    {
        return new DataportOptions()
        {
            DataportApiToken = options.DataportApiToken,
            DataportApiAddress = options.DataportApiAddress,
            DataportConnectionstring = options.DataportConnectionstring,
            AzureBlobConnectionstring = options.AzureBlobConnectionstring,
            AzureBlobContainer = options.AzureBlobContainer,
            AzureBlobHost = options.AzureBlobHost
        };
    }

}