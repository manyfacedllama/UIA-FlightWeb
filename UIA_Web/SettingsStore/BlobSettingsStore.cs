using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace UIA_Web.SettingsStore
{

        public class BlobSettingsStore : ISettingsStore
        {
            private readonly CloudBlockBlob configBlob;

            public BlobSettingsStore() : this(CloudConfigurationManager.GetSetting("externalconnectionuia_AzureStorageConnectionString"), "configuration", HttpContext.Current.Server.MapPath("~/Web.config"))
            {
            }

            public BlobSettingsStore(string storageAccount, string configContainer, string configBlobName)
            {
                var account = CloudStorageAccount.Parse(storageAccount);
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(configContainer);

                this.configBlob = container.GetBlockBlobReference(configBlobName);
            }

            public string GetVersion()
            {
                this.configBlob.FetchAttributes();

                return this.configBlob.Properties.ETag;
            }

            public Dictionary<string, string> FindAll()
            {
                return  this.ReadSettingsFromStorage();
            }

            private Dictionary<string, string> ReadSettingsFromStorage()
            {
                XElement configFile;

                // Read the configuration blob and return the settings as a Dictionary.
                using (var stream = new MemoryStream())
                {
                    this.configBlob.DownloadToStream(stream);

                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        configFile = XElement.Parse(reader.ReadToEnd());
                    }
                }
            return configFile.Element("connectionStrings").Descendants().ToDictionary(x=>x.Attribute("name").Value,x=>x.Attribute("connectionString").Value);
            }

    }


}