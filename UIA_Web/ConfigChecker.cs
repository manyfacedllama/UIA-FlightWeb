using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System.Web.Configuration;

namespace UIA_Web
{


        public class ConfigChecker
        {
        private const string configContainer = "configuration";
        private string ConfigBlobNameApp = HttpContext.Current.Server.MapPath("~/Web.config");
        public void Run()
            {
            ExternalConfiguration.Instance.ConfigChangeMonitor();
            System.Configuration.Configuration conf = WebConfigurationManager.OpenWebConfiguration("/");
            conf.ConnectionStrings.ConnectionStrings["UIADbContext"].ConnectionString = ExternalConfiguration.Instance.GetAppSetting("UIADbContext");
            conf.ConnectionStrings.ConnectionStrings["UIA_Entities"].ConnectionString = ExternalConfiguration.Instance.GetAppSetting("UIA_Entities");
            conf.Save();
        }

            private void UploadConfigurationBlob()
            {
                var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("externalconnectionuia_AzureStorageConnectionString"));
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(configContainer);
                container.CreateIfNotExists();

                var productionBlob = container.GetBlockBlobReference(ConfigBlobNameApp);
                productionBlob.UploadFromFile(ConfigBlobNameApp);
            }

            private void DeleteConfigurationBlob()
            {
                var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("externalconnectionuia_AzureStorageConnectionString"));
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(configContainer);

                container.DeleteIfExists();
            }
        }

}