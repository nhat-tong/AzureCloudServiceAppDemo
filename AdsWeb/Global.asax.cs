using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AdsWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create blob and queue container if they don't already exists
            CreateCloudContainers();
        }

        private void CreateCloudContainers()
        {
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();
            var imageContainer = blobClient.GetContainerReference("images");
            if(imageContainer.CreateIfNotExists())
            {
                imageContainer.SetPermissions(new BlobContainerPermissions {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            var queueClient = storageAccount.CreateCloudQueueClient();
            var queueContainer = queueClient.GetQueueReference("images");
            queueContainer.CreateIfNotExists();
        }
    }
}
