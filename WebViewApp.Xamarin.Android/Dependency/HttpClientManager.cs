using System;
using System.Net;
using System.Net.Http;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Droid.Dependency;
using Xamarin.Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientManager))]
namespace WebViewApp.Xamarin.Droid.Dependency
{
    public class HttpClientManager : IHttpClientManager
    {
        public HttpClientManager()
        {
        }

        public HttpClient GetHttpClientForAuthentication()
        {
            var client = new HttpClient();

            return client;
        }

        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                var handler = new AndroidClientHandler
                {
                    UseCookies = true,
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                };

                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                client = new HttpClient(new AndroidClientHandler());
            }
            return client;
        }
    }
}
