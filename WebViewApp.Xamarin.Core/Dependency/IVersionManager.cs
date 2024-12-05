using System;
namespace WebViewApp.Xamarin.Core.Dependency
{
    public interface IVersionManager
    {
        String VersionNumber();
        String BuildNumber();
    }
}
