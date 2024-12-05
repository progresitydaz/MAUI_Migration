using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.iOS.Dependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(SmsBroadcastManager))]
namespace WebViewApp.Xamarin.iOS.Dependency
{
    public class SmsBroadcastManager : ISmsBroadcastManager
    {
        public void RegisterSmsReceiver()
        {
            //TODO implement feature
        }

        public void UnRegisterSmsReceiver()
        {
            //TODO implement feature
        }
    }
}
