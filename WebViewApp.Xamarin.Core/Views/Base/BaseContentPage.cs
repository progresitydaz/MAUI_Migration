using System;
using System.Diagnostics;
using WebViewApp.Xamarin.Core.Dependency;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views.Base
{

    public partial class BaseContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected void SetSafeArea()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var platformManager = DependencyService.Get<IPlatformManager>();

                var top = platformManager.GetSafeAreaInset();

                this.Padding = new Thickness(0, top * 0.8, 0, 0);
            }
        }
    }
}
