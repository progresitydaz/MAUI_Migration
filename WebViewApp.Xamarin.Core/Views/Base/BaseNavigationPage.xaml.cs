using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views.Base
{
    public partial class BaseNavigationPage : NavigationPage
    {
        public BaseNavigationPage()
        {
            InitializeComponent();
        }

        public BaseNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}
