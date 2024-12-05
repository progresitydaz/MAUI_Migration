using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using WebViewApp.Xamarin.Core.Extensions;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.iOS.Renderers;
using UIKit;
using WebKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(ExtendedWebViewRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class ExtendedWebViewRenderer : WkWebViewRenderer
    {
        WKWebView _wkWebView;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ScrollView.Bounces = false; // Disable bounce
            }

            if (NativeView != null)
            {
                var webView = (WKWebView)NativeView;

                webView.Configuration.Preferences.JavaScriptEnabled = true;
                webView.Configuration.Preferences.JavaScriptCanOpenWindowsAutomatically = true;
                webView.Configuration.WebsiteDataStore = WKWebsiteDataStore.DefaultDataStore;

                webView.UIDelegate = new ExtendedUIDelegate();

                webView.NavigationDelegate = new ExtendedNavigatioDelegate();
            }
        }

        private class ExtendedUIDelegate : WKUIDelegate
        {
            public override WebKit.WKWebView CreateWebView(WebKit.WKWebView webView, WebKit.WKWebViewConfiguration configuration, WebKit.WKNavigationAction navigationAction, WebKit.WKWindowFeatures windowFeatures)
            {
                var url = navigationAction.Request.Url;

                Launcher.OpenAsync(url); // Open links in the system browser

                return null;
            }
        }

        private class ExtendedNavigatioDelegate : WKNavigationDelegate
        {
            public override void DidReceiveServerRedirectForProvisionalNavigation(WKWebView webView, WKNavigation navigation)
            {
                // Log or handle the server redirect here
                // Note: The current URL can be retrieved from the webView's URL property

                var currentUrl = webView.Url?.AbsoluteString;

                // Check the URL to determine if any specific action is needed
                if (currentUrl != null)
                {
                    Console.WriteLine($"Redirected to: {currentUrl}");

                    // Handle specific redirects
                    if (currentUrl.Contains("redirect-url"))
                    {
                        // Perform actions when redirecting to a specific URL
                        // For example, we can set cookies, show an alert, or load a different URL

                        // Set cookies here if needed
                        // var cookie = new NSHttpCookie("name", "value", ".domain.com", "/");
                        // WKWebsiteDataStore.DefaultDataStore.HttpCookieStore.SetCookie(cookie, null);

                        // Optionally load another URL
                        // webView.LoadRequest(new NSUrlRequest(new NSUrl("new-url")));
                    }
                }

                // Optionally, we can call other methods or perform actions based on the redirect
                // e.g., logging, updating the UI, etc.
            }

            public override async void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                var url = navigationAction.Request?.Url?.ToString();

                Debug.WriteLine("****url****" + url);

                NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;

                //// Check if the URL is the Microsoft login URL we want to intercept
                if (url == "https://login.microsoftonline.com/f5059662-bafc-4263-a5ba-a89bef4641b8/login")
                {
                    await Task.Delay(500);

                    var cookieStore = WKWebsiteDataStore.DefaultDataStore.HttpCookieStore;

                    cookieStore.GetAllCookies(cookies =>
                    {
                        foreach (var cookie in cookies)
                        {
                            Console.WriteLine($"Cookie before: {cookie.Name} = {cookie.Value}");
                        }
                    });

                    // Add existing cookies to the WebView manually

                    foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                    {
                        webView.Configuration.WebsiteDataStore.HttpCookieStore.SetCookie(cookie, null);
                    }

                    cookieStore.GetAllCookies(cookies =>
                    {
                        foreach (var cookie in cookies)
                        {
                            Console.WriteLine($"Cookie after: {cookie.Name} = {cookie.Value}");
                        }
                    });
                }

                bool isRedirected = UriHelper.ValidateUrlRedirect(url);

                if (isRedirected)
                {
                    await Launcher.OpenAsync(url); // Handle redirects if necessary

                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                else
                {
                    decisionHandler(WKNavigationActionPolicy.Allow);
                }
            }
        }
    }
}
