using System;
using System.IO;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Localization;
using Plugin.Media;
using Plugin.Media.Abstractions;
using WebViewApp.Xamarin.Core.Services;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IFileSystemService
    {
        Task<MediaFile> PickPhotoAsync();

        Task<MediaFile> TakePhotoAsync();

        ImageSource GetImageSource(MediaFile mediaFile);

        Task<string> GetBase64ImageSource(MediaFile mediaFile);
    }

    public class FileSystemService : IFileSystemService
    {
        private readonly IDialogService _dialogService;

        public FileSystemService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public ImageSource GetImageSource(MediaFile mediaFile)
        {
            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = mediaFile.GetStream();
                return stream;
            });

            return imageSource;
        }

        public async Task<string> GetBase64ImageSource(MediaFile mediaFile)
        {
            var stream = mediaFile.GetStreamWithImageRotatedForExternalStorage();

            var bytes = new byte[stream.Length];

            await stream.ReadAsync(bytes, 0, (int)stream.Length);

            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            MediaFile file = null;

            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await _dialogService.ShowToast(AppResources.PickPhotoError);
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Custom,
                        CustomPhotoSize = 40,
                        RotateImage = true,
                        CompressionQuality = 40,
                        SaveMetaData = false
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException($"Exception occured TakePhotoAsync {0}", ex);
            }

            return file;
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            MediaFile file = null;

            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await _dialogService.ShowToast(AppResources.PickPhotoError);
                }
                else
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Stembureau",
                        Name = "photo.jpg",
                        SaveToAlbum = true,
                        AllowCropping = false,
                        RotateImage = true,
                        PhotoSize = PhotoSize.Custom,
                        CustomPhotoSize = 40,
                        CompressionQuality = 40,
                        SaveMetaData = false
                    });
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowToast(AppResources.PickPhotoNotSupported);

                LogHelper.LogException($"Exception occured TakePhotoAsync {0}", ex);
            }

            return file;
        }
    }
}
