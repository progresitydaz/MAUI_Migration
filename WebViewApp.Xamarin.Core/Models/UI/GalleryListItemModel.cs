using System;
using System.IO;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Extensions;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class GalleryListItemModel : ListItemModel
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string Id { get; set; }

        public bool IsDefault { get; set; }

        ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                RaisePropertyChanged(() => ImageSource);
            }
        }

        private string imageBase64;
        public string ImageBase64
        {
            get { return imageBase64; }
            set
            {
                imageBase64 = value;
                RaisePropertyChanged(() => ImageBase64);

                ImageSource = ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(imageBase64)));
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                RaisePropertyChanged(() => ImageUrl);

                if (_imageUrl.IsURI())
                {
                    ImageSource = ImageSource.FromUri(new Uri(_imageUrl));
                }
            }
        }

        public static GalleryListItemModel FromIcon(string icon, bool isDefault = false)
        {
            var imageSource = new GalleryListItemModel()
            {
                IconSource = icon,
                FileName = string.Empty,
                MimeType = Path.GetExtension(icon),
                IsDefault = isDefault,
            };

            return imageSource;
        }

        public async static Task<GalleryListItemModel> FromMediaFile(MediaFile mediaFile)
        {
            var fileSystemService = AppDependencyResolver.Resolve<IFileSystemService>();
            string imageSourceBase64 = await fileSystemService.GetBase64ImageSource(mediaFile);
            string fileName = Path.GetFileName(mediaFile.Path);
            string mimeType = Path.GetExtension(mediaFile.Path);

            mimeType = mimeType.Replace("jpg", @"jpeg");

            mimeType = mimeType.Replace(".", @"image/");

            var imageSource = new GalleryListItemModel()
            {
                ImageBase64 = imageSourceBase64,
                FileName = fileName,
                MimeType = mimeType,
            };

            return imageSource;
        }

        public static GalleryListItemModel FromBase64(string imageSourceBase64)
        {
            var imageSource = new GalleryListItemModel()
            {
                ImageBase64 = imageSourceBase64,
                FileName = string.Empty,
                MimeType = string.Empty,
            };

            return imageSource;
        }
    }
}
