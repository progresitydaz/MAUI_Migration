using System;
using System.IO;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Droid.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]  
namespace WebViewApp.Xamarin.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
