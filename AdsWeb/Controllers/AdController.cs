using AdsCommon;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdsWeb.Controllers
{
    public class AdController : Controller
    {
        private CloudBlobContainer _imageBlobContainer;
        private CloudQueue _imageQueueContainer;
        private ContosoAdsContext _dbContext;

        public AdController()
        {
            _dbContext = new ContosoAdsContext();
            InitializeStorage();
        }

        private void InitializeStorage()
        {
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            _imageBlobContainer = blobClient.GetContainerReference("images");

            var queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            _imageQueueContainer = queueClient.GetQueueReference("images");
        }

        // GET: Ad
        public async Task<ActionResult> Index(int? category)
        {
            var adsList = _dbContext.Ads.AsQueryable();
            if (category != null)
            {
                adsList = adsList.Where(a => a.Category == (Category)category);
            }

            return View(await adsList.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ad ad = await _dbContext.Ads.FindAsync(id);
            if (ad == null)
            {
                return HttpNotFound();
            }

            return View(ad);
        }

        // GET
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include ="Title,Price,Description,Category,Phone")] Ad ad, HttpPostedFileBase imageFile)
        {
            if(ModelState.IsValid)
            {
                // upload image to blob
                CloudBlockBlob imageBlob = null;
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    imageBlob = await UploadAndSaveBlobAsync(imageFile);
                    ad.ImageURL = imageBlob.Uri.ToString();
                }

                // save record to database
                ad.PostedDate = DateTime.Now;
                _dbContext.Ads.Add(ad);
                await _dbContext.SaveChangesAsync();
                Trace.TraceInformation("Created AdId {0} in database", ad.AdId);

                // create a queue message to inform the back-end
                if (imageBlob != null)
                {
                    var queueMessage = new CloudQueueMessage(ad.AdId.ToString());
                    await _imageQueueContainer.AddMessageAsync(queueMessage);
                    Trace.TraceInformation("Created queue message for AdId {0}", ad.AdId);
                }

                return RedirectToAction("Index");
            }

            return View(ad);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ad ad = await _dbContext.Ads.FindAsync(id);
            if (ad == null)
            {
                return HttpNotFound();
            }

            return View(ad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdId,Title,Price,Description,ImageURL,ThumbnailURL,PostedDate,Category,Phone")] Ad ad, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // upload image to blob
                CloudBlockBlob imageBlob = null;
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    await DeleteAdBlobAsync(ad);

                    imageBlob = await UploadAndSaveBlobAsync(imageFile);
                    ad.ImageURL = imageBlob.Uri.ToString();
                }

                // save record to database
                ad.PostedDate = DateTime.Now;
                _dbContext.Entry(ad).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                Trace.TraceInformation("Updated AdId {0} in database", ad.AdId);

                // create a queue message to inform the back-end
                if (imageBlob != null)
                {
                    var queueMessage = new CloudQueueMessage(ad.AdId.ToString());
                    await _imageQueueContainer.AddMessageAsync(queueMessage);
                    Trace.TraceInformation("Created queue message for AdId {0}", ad.AdId);
                }

                return RedirectToAction("Index");
            }

            return View(ad);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ad ad = await _dbContext.Ads.FindAsync(id);
            if (ad == null)
            {
                return HttpNotFound();
            }

            return View(ad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var ad = await _dbContext.Ads.FindAsync(id);
            // Delete image from blob
            await DeleteAdBlobAsync(ad);
            // Remove ad from db
            _dbContext.Ads.Remove(ad);
            await _dbContext.SaveChangesAsync();

            Trace.TraceInformation("Deleted ad {0}", ad.AdId);
            return RedirectToAction("Index");
        }

        private async Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile)
        {
            Trace.TraceInformation("Uploading image file {0}", imageFile.FileName);
            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var imageBlob = _imageBlobContainer.GetBlockBlobReference(blobName);
            
            using(var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            return imageBlob;
        }

        private async Task DeleteAdBlobAsync(Ad ad)
        {
            if(!string.IsNullOrWhiteSpace(ad.ImageURL))
            {
                var blobUri = new Uri(ad.ImageURL);
                await DeleteAdBlobFromUriAsync(blobUri);
            }

            if(!string.IsNullOrWhiteSpace(ad.ThumbnailURL))
            {
                var blobUri = new Uri(ad.ThumbnailURL);
                await DeleteAdBlobFromUriAsync(blobUri);
            }
        }

        private async Task DeleteAdBlobFromUriAsync(Uri blobUri)
        {
            var blobName = blobUri.Segments[blobUri.Segments.Length - 1];
            Trace.TraceInformation("Deleting image blob {0}", blobName);
            var blobToDelete = _imageBlobContainer.GetBlockBlobReference(blobName);
            await blobToDelete.DeleteIfExistsAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}