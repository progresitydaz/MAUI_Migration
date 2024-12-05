using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class NotificationPopupView : ContentView
    {
        public PushNotificationData Notification
        {
            get { return (PushNotificationData)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        public static readonly BindableProperty NotificationProperty =
        BindableProperty.Create("Notification",
                               typeof(PushNotificationData),
                               typeof(NotificationPopupView),
                               null,
                               BindingMode.OneWay);



        public NotificationPopupView()
        {
            InitializeComponent();

            PropertyChanged += Component_PropertyChanged;
        }

        private void Component_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Notification))
            {
                if (Notification != null)
                {
                    PopupHeaderText.Text = Notification.Title;
                    PopupDetailText.Text = Notification.Body;
                }
            }
        }

        private void Close_ButtonClcked(object sender, System.EventArgs e)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            var vm = BindingContext as ViewModelBase;

            vm.IsNotificationAvailable = false;
        }

        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var vm = BindingContext as ViewModelBase;

            GlobalSetting.Instance.NewMessagesNotification = null;

            await vm.NavigateToMenu(PageType.Chat);
        }
    }
}
