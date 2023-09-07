/*
using System;
using CommandLine;
using Microsoft.Extensions.Logging;
using Azlon.DataPort.Client;
public class Subscribe : IDisposable
{
    private readonly ILogger<Subscribe> logger;
    private readonly SubscribeOptions options;
    private bool disposedValue;

    public Subscribe(ILogger<Subscribe> logger, SubscribeOptions options)
    {
        this.logger = logger;
        this.options = options;
    }

    public void Run()
    {


    }

    public int RunAndReturnExitCode()
    {
        return 1; // on error
        return 0; // on success
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
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
*/