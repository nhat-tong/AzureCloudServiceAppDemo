using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using AdsCommon;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AdsWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private ContosoAdsContext _dbContext = null;
        private CloudBlobContainer _imagesBlob = null;
        private CloudQueue _imagesQueue = null;

        public override void Run()
        {
            Trace.TraceInformation("AdsWorker is running");
            CloudQueueMessage msg = null;

            while (true)
            {
                try
                {
                    msg = _imagesQueue.GetMessage();
                    if (msg != null)
                    {
                        ProcessQueueMessage(msg);
                    }
                    else
                    {
                        // IMPORTANT: this sleep prevents the worker role from incurring excessive CPU time and storage transaction costs
                        Thread.Sleep(1000);
                    }
                }
                catch (StorageException e)
                {
                    if (msg != null && msg.DequeueCount > 5)
                    {
                        _imagesQueue.DeleteMessage(msg);
                        Trace.TraceError("Deleting poison queue item: '{0}'", msg.AsString);
                    }
                    Trace.TraceError("Exception in AdsWorker: '{0}'", e.Message);
                    Thread.Sleep(5000);
                }
            }

        }

        private void ProcessQueueMessage(CloudQueueMessage msg)
        {
            Trace.TraceInformation("Processing queue message {0}", msg);

            var adId = int.Parse(msg.AsString);
            var ad = _dbContext.Ads.Find(adId);
            if(ad == null)
            {
                throw new Exception(string.Format("AdId {0} not found, can't create thumbnail", adId.ToString()));
            }

            var blobUri = new Uri(ad.ImageURL);
            string blobName = blobUri.Segments[blobUri.Segments.Length - 1];
            var imageInputBlob = _imagesBlob.GetBlockBlobReference(blobName);

            var thumbnailName = Path.GetFileNameWithoutExtension(imageInputBlob.Name) + "thumb.jpg";
            var imageOutputBlob = _imagesBlob.GetBlockBlobReference(thumbnailName);

            using (var inputStream = imageInputBlob.OpenRead())
                using(var outputStream = imageOutputBlob.OpenWrite())
            {
                ConvertImageToThumbnail(inputStream, outputStream);
                imageOutputBlob.Properties.ContentType = "image/jpeg";
            }
            Trace.TraceInformation("Generated thumbnail in blob {0}", thumbnailName);

            // save to db
            ad.ThumbnailURL = imageOutputBlob.Uri.ToString();
            _dbContext.Entry(ad).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();
            Trace.TraceInformation("Updated thumbnail URL in database: {0}", ad.ThumbnailURL);

            // Remove message from queue.
            _imagesQueue.DeleteMessage(msg);
        }

        public void ConvertImageToThumbnail(Stream input, Stream output)
        {
            int thumbnailsize = 80;
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = thumbnailsize;
                height = thumbnailsize * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = thumbnailsize;
                width = thumbnailsize * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;
            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // Read database connection string and open database
            var dbConnString = CloudConfigurationManager.GetSetting("ContosoAdsDbConnectionString");
            _dbContext = new ContosoAdsContext(dbConnString);

            // Create blob and queue containers if they doesn't exists
            CreateCloudContainers();

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("AdsWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            _dbContext.Dispose();

            base.OnStop();

            Trace.TraceInformation("AdsWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

        private void CreateCloudContainers()
        {
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            Trace.TraceInformation("Creating images blob container");
            var blobClient = storageAccount.CreateCloudBlobClient();
            _imagesBlob = blobClient.GetContainerReference("images");
            if (_imagesBlob.CreateIfNotExists())
            {
                _imagesBlob.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            Trace.TraceInformation("Creating images queue");
            var queueClient = storageAccount.CreateCloudQueueClient();
            _imagesQueue = queueClient.GetQueueReference("images");
            _imagesQueue.CreateIfNotExists();

            Trace.TraceInformation("Storage initialized");
        }
    }
}
