
using Azlon.Dataport.Model;
using Microsoft.Extensions.Logging;
using System.Xml;

public class Notifications
{
    private readonly ConsoleOptions consoleOptions;
    private readonly ILogger<Notifications> logger;

    public Notifications(
        ILogger<Notifications> logger,
        ConsoleOptions consoleOptions)
    {
        this.logger = logger;
        this.consoleOptions = consoleOptions;
    }

    public void OnNotificationReceived(object source, Notification message)
    {
        // TODO: write logic for file naming pattern
        // TODO: optionally write logic for subfiles. when the file is a 'product xml' the xml contains information about related (versioned) files, these also need to be downloaded and saved to disk.
        // do this inan abstract manner to avoid configration in code (use configuration do this this business logic, e.g. configure a list of xpath for additional downloads, and a filename regex)
        logger.LogDebug($"received message with identitfier: {message.MessageIdentifier}.");
        if (message != null)
        {
            logger.LogDebug($"message: {message}.");
            var uri = new Uri(message.DataContainer.Uri.ToString());
            var fileName = Path.GetFileName(message.DataContainer.Uri.ToString());

            try
            {
                logger.LogInformation($"Downloading and saving file for message {message.MessageIdentifier}.");
                Utilities.DownloadAndSaveToFileAsync(uri, consoleOptions.OutFolder, fileName).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
            if (consoleOptions.ExtraFilesXpath.Any())
            {
                ProcessExtraFilesXpath(consoleOptions.OutFolder, fileName).GetAwaiter().GetResult();
            }
        }
    }

    public async Task ProcessExtraFilesXpath(string outFolder, string fileName)
    {
        logger.LogInformation($"Processing extra files xpath for file {fileName}.");

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(Path.Join(outFolder, fileName).ToString());
        foreach (var xpath in consoleOptions.ExtraFilesXpath)
        {
            var xpaths = xpath.Split(';');
            var itemXpath = xpaths[0];
            var urlXpath = xpaths[1];
            var filenameXpath = xpaths[2];
            // i don't know if it's an attribute or node
            var nodes = xmlDocument.SelectNodes(itemXpath);
            if (nodes != null && nodes.Count > 0)
            {
                // fileitem-xpath-node;fileurl-xpath-nodeORattr;filename-xpath-nodeORattr
                foreach (XmlNode node in nodes)
                {
                    string url;
                    var urlNode = node.SelectSingleNode(urlXpath);
                    if (urlNode is XmlAttribute urlAttribute)
                    {
                        url = urlAttribute.Value;
                    }
                    else
                    {
                        url = urlNode.InnerText;
                    }

                    string filename;
                    var nameNode = node.SelectSingleNode(filenameXpath);
                    if (nameNode is XmlAttribute nameAttribute)
                    {
                        filename = nameAttribute.Value;
                    }
                    else
                    {
                        filename = nameNode.InnerText;
                    }

                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = Path.GetFileName(urlXpath);
                    }

                    logger.LogInformation($"Extracted url {url} from item Xpath {itemXpath} and url xpath {urlXpath} and trying to download.");
                    try
                    {
                        // TODO: download from blob? and choose filename (also xpath?)
                        await Utilities.DownloadAndSaveToFileAsync(new Uri(url), outFolder, filename);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
                    }
                }
            }
        }
        xmlDocument = null;
    }
}
