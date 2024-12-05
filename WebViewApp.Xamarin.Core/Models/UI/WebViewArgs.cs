using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebViewApp.Xamarin.Core.Models
{
    public class WebViewArgs
    {
        public string Url { get; set; }

        public string PageTitle { get; set; }

        public WebViewArgs()
        {
        }
    }
}
