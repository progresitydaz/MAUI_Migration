using System;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.Models.Base;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class RadioButtonItemModel : ExtendedBindableObject
    {
        public string Caption { get; set; }

        public object Tag { get; set; }

        public object Identfier { get; set; }

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
                RaisePropertyChanged(() => ImageSource);
            }
        }

        public string ImageSource
        {
            get
            {
                var image = IsSelected ? "RadioButton_Checked.png" : "RadioButton_Unchecked.png";
                return image;
            }
        }

        public List<RadioButtonItemModel> ListItems { get; internal set; }
    }
}

