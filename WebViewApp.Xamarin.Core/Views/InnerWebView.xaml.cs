using System;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.ViewModels;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using WebViewApp.Xamarin.Core.Views.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class InnerWebView : BaseContentPage
    {
        public InnerWebView()
        {
            InitializeComponent();

            SetSafeArea();
        }

        protected async override void OnAppearing()
        {
            var vm = BindingContext as ViewModelBase;

            if (vm != null)
            {
                vm.UnSubscribeToEvents();
                vm.SubscribeToEvents();
                await vm.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            var vm = BindingContext as ViewModelBase;

            vm?.UnSubscribeToEvents();

            vm?.OnDisappearing();
        }

        private void Webview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var vm = BindingContext as InnerWebViewModel;

            vm.WebViewNavigating(sender, e);
        }

        private async void Webview_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var vm = BindingContext as InnerWebViewModel;

            string jsHeader = "javascript:(function() { " +
                       "var node = document.getElementsByTagName('header')[0];"
                       + "node.parentNode.removeChild(node);" +
                       "})()";

            string jsFooter = "javascript:(function() { " +
                     "var node = document.getElementsByTagName('footer')[0];"
                     + "node.parentNode.removeChild(node);" +
                     "})()";

            await WebViewInner.EvaluateJavaScriptAsync(jsHeader);

            await WebViewInner.EvaluateJavaScriptAsync(jsFooter);

            vm.WebViewNavigated(sender, e);
        }
    }
}
