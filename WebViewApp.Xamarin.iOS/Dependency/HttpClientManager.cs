using System;
using System.Net.Http;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.iOS.Dependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientManager))]
namespace WebViewApp.Xamarin.iOS.Dependency
{
    public class HttpClientManager : IHttpClientManager
    {
        public HttpClientManager()
        {
        }

        public HttpClient GetHttpClientForAuthentication()
        {
            var client = new HttpClient(new NSUrlSessionHandler()); ;

            return client;
        }

        public HttpClient GetHttpClient()
        {
            var client = new HttpClient(new NSUrlSessionHandler());

            return client;
        }
    }
}
