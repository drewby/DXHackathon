using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // parse query parameter
            string AzureStorageConnString = "DefaultEndpointsProtocol=https;AccountName=dxhachathontempei7242;AccountKey=S55u2Oq1yB4yOReFB9Dkkz/yH94T5kQlLGGP2LVcQCDGqGQA4NHRBQnOsuue8+ZrzR9YSwQfmE3zxJ1/WvborQ==";
            var cloudStorageAccount = CloudStorageAccount.Parse(AzureStorageConnString);
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobs = blobClient.GetContainerReference("photos").ListBlobs();

            var lastUpdated = new DateTimeOffset();
            var returnUri = "";
            foreach (var item in blobs )
            {
                var currentUpdated = (DateTimeOffset)((CloudBlockBlob)item).Properties.LastModified;
                if (lastUpdated < currentUpdated)
                {
                    lastUpdated = currentUpdated;
                    returnUri = ((CloudBlockBlob)item).Uri.ToString();
                }
            }

            return req.CreateResponse(HttpStatusCode.OK, returnUri);
}