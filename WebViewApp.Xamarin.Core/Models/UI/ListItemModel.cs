using System;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.Models.Base;
using WebViewApp.Xamarin.Core.ViewModels.Base;

namespace WebViewApp.Xamarin.Core.Models
{
    public class ListItemModel : ExtendedBindableObject, ISelectable
    {
        public string Caption { get; set; }

        public string Header { get; set; }

        public string SubHeader { get; set; }

        public string IconSource { get; set; }

        public string VisibleItems { get; set; }

        public Type ViewModel { get; set; }

        public object Tag { get; set; }

        public Type CellType { get; set; }

        public object Identfier { get; set; }

        public List<ListItemModel> Siblings { get; set; }

        bool _isSelected;
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
            }
        }

        public ListItemModel()
        {
            Siblings = new List<ListItemModel>();
        }

    }
}

