using System;
using Foundation;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.iOS.Dependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionManager))]
namespace WebViewApp.Xamarin.iOS.Dependency
{
    public class VersionManager: IVersionManager
    {
        public VersionManager()
        {
        }

        public string BuildNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
        }

        public string VersionNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }
    }
}
