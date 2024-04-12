using Azlon.Dataport.Client;
public static class Extensions
{
    public static DataportOptions ToDataportOptions(this ConsoleOptions options)
    {
        return new DataportOptions()
        {
            Environment = options.Environment,
            Name = options.Name,
            ApiAddress = options.ApiAddress,
            Connectionstring = options.Connectionstring,
            SharedAccessKey = options.SharedAccessKey,
            DeviceId = options.DeviceId,
            AzureBlobContainer = options.AzureBlobContainer,
            AzureBlobHost = options.AzureBlobHost,
        };
    }
}
