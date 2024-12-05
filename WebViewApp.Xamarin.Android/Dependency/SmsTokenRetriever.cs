using System.Diagnostics;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Tasks;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Droid.Dependency;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(SmsTokenRetriever))]
namespace WebViewApp.Xamarin.Droid.Dependency
{
    public class SmsTokenRetriever : ISmsTokenRetriever
    {
        public void ListenToSmsRetriever()
        {
            // Get an instance of SmsRetrieverClient, used to start listening for a matching
            // SMS message.
            SmsRetrieverClient client = SmsRetriever.GetClient(Application.Context);
            // Starts SmsRetriever, which waits for ONE matching SMS message until timeout
            // (5 minutes). The matching SMS message will be sent via a Broadcast Intent with
            // action SmsRetriever#SMS_RETRIEVED_ACTION.
            var task = client.StartSmsRetriever();
            // Listen for success/failure of the start Task. If in a background thread, this
            // can be made blocking using Tasks.await(task, [timeout]);
            task.AddOnSuccessListener(new SuccessListener());
            task.AddOnFailureListener(new FailureListener());
        }

        private class SuccessListener : Java.Lang.Object, IOnSuccessListener
        {
            public void OnSuccess(Java.Lang.Object result)
            {
                Debug.WriteLine("Sms Retriever listener succeeded.....");
            }
        }

        private class FailureListener : Java.Lang.Object, IOnFailureListener
        {
            public void OnFailure(Java.Lang.Exception ex)
            {
                LogHelper.LogException("Sms Retriever listener failed", ex);
            }
        }
    }
}
