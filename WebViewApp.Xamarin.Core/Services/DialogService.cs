using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IDialogService
    {
        Task ShowDialog(string message, string title, string buttonLabel);

        Task<bool> ShowConfirmation(string message, string okLabel, string cancelLabel);

        Task ShowToast(string message);

        Task<string> DisplayActionSheet(string title, string cancel, string destructive, string[] buttons);

        List<ListItemModel> GetCameraActionsheetItems();
    }

    public class DialogService : BaseService, IDialogService
    {
        public Task<bool> ShowConfirmation(string message, string okLabel, string cancelLabel)
        {
            ConfirmConfig config = new ConfirmConfig()
            {
                Message = message,
                OkText = okLabel,
                CancelText = cancelLabel,
            };

            var result = UserDialogs.Instance.ConfirmAsync(config);

            return result;
        }

        public Task ShowDialog(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public async Task ShowToast(string message)
        {
            UserDialogs.Instance.Toast(message, TimeSpan.FromSeconds(3));

            await Task.FromResult(true);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destructive, string[] buttons)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, null, buttons);

            return result;
        }

        public List<ListItemModel> GetCameraActionsheetItems()
        {
            List<ListItemModel> listItems = new List<ListItemModel>()
            {
                new ListItemModel()
                {
                    ViewModel = null,
                    Header = AppResources.MakePhoto,
                    Identfier = 1,
                    IconSource = "Photo_camera.png",
                },
                new ListItemModel()
                {
                    ViewModel = null,
                    Header = AppResources.ChooseFromPhotos,
                    Identfier = 2,
                    IconSource = "Galery.png",
                }
            };

            return listItems;
        }
    }
}
