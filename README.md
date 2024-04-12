This repository contains source code and distributions of the DataPorts command line interface tool.

# Getting started
The source code from this repository shows a .Net sample on how to integrate the Azlon.DataPort.Client package.

The CLI tool can also be used to receive files from a DataPort.

Get the latest version [here](https://github.com/Azlon-io/dataport-cli/tags).

Another sample application can be found [here](https://github.com/Azlon-io/DataPorts)

## Implement the Azlon.DataPort.Client package
### Configuration
Use the .env.example file to create a .env file in the solution folder. After that, provide values for 

DP_OUT: Path where received files are stored
DP_LOGLEVEL: Information|Warning|Error
DP_ENV: Test|Live
DP_NAME: Name of the DataPort
DP_DEVICEID: Unique DataPort identifier, supplied by your contact person
DP_SHAREDACCESSKEY: Access key for the DataPort, supplied by your contact person

### Receiving files
First the OnNotificationReceived event must be subscribed to, after that the StartReceivingMessages() must be called to listen for files on the DataPort.
```
var client = serviceProvider.GetRequiredService<DataportClient>();
client.OnNotificationReceived += notifications.OnNotificationReceived;
client.StartReceivingMessages(); 
```

### Sending files
For now only receiving files from a DataPort is implemented in the CLI tool.

However, implementing sending files is easily implemented by calling method SendDataportContainerAsync() on the DataportClient class.

## Using the CLI tool for receiving files
- Download the latest version of the CLI tool for Windows (azlonctl-win-x64.zip) or Linux (azlonctl-linux-x64.zip)
- Run the tool by providing the configuration parameters, for example:
```
azlonctl.exe --out="/dataport-output" --loglevel=Information --environment=Test --name=Test DataPort --deviceId=??? --sharedaccesskey=???
```
