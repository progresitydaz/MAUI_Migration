using System;
using System.Net.Http;

namespace WebViewApp.Xamarin.Core.Dependency
{
    public interface IHttpClientManager
    {
        HttpClient GetHttpClient();
        HttpClient GetHttpClientForAuthentication();
    }
}
