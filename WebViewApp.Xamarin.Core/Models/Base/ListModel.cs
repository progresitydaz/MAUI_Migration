using System;
using System.ComponentModel;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.ViewModels.Base;

namespace WebViewApp.Xamarin.Core.Models.Base
{
    public class ListModel : ExtendedBindableObject
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public Type ModelType { get; set; }

        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
                RaisePropertyChanged(() => CheckedImage);
            }
        }

        public string CheckedImage
        {
            get
            {
                return IsSelected ? "TickMark.png" : "";
            }
        }

    }
}
