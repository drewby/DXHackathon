using System;
using System.Diagnostics;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace TempeiCapture
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        MediaCapture mediaCaptureManager;
        StorageFile photoStorageFile;
        DispatcherTimer uploadTimer;
        string capturedPhotoFile = "CapturedPhoto.jpg";
        private int UploadIntervalPerSec = 5;
        private static string AzureStorageConnString = "DefaultEndpointsProtocol=https;AccountName=dxhachathontempei7242;AccountKey=S55u2Oq1yB4yOReFB9Dkkz/yH94T5kQlLGGP2LVcQCDGqGQA4NHRBQnOsuue8+ZrzR9YSwQfmE3zxJ1/WvborQ==";


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            mediaCaptureManager = new MediaCapture();
            try
            {
                await mediaCaptureManager.InitializeAsync();
                previewElement.Source = mediaCaptureManager;
                await mediaCaptureManager.StartPreviewAsync();

                InitializeTimer();
                uploadTimer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            //throw new NotImplementedException();
        }


        private void InitializeTimer()
        {
            if (uploadTimer == null)
            {
                uploadTimer = new DispatcherTimer();
                uploadTimer.Interval = TimeSpan.FromSeconds(UploadIntervalPerSec);
                uploadTimer.Tick += (o, s) =>
                {
                    UploadPhoto();
                };
            }
        }

        private async void UploadPhoto()
        {
            uploadTimer.Stop();
            photoStorageFile = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync(capturedPhotoFile, Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();
            try
            {
                await mediaCaptureManager.CapturePhotoToStorageFileAsync(imageProperties, photoStorageFile);
                var cloudStorageAccount = CloudStorageAccount.Parse(AzureStorageConnString);
                var blobClient = cloudStorageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("photos");
                await container.CreateIfNotExistsAsync();
                var fileName = "photo" +DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromFileAsync(photoStorageFile);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            uploadTimer.Start();
        }



    }
}
