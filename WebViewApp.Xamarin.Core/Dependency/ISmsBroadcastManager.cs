using System;
namespace WebViewApp.Xamarin.Core.Dependency
{
    public interface ISmsBroadcastManager
    {
        void RegisterSmsReceiver();
        void UnRegisterSmsReceiver();
    }
}
