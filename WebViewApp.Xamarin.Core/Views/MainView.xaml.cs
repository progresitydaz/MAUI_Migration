using System;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class MainView : FlyoutPage
    {
        public MainView()
        {
            InitializeComponent();
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
            vm?.OnDisappearing();
        }
    }
}
