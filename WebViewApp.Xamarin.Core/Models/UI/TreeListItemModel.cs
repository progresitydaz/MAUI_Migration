using System;
using System.Collections.Generic;
using System.Linq;
using WebViewApp.Xamarin.Core.Extensions;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class TreeListItemModel : ListItemModel
    {
        private Style _normalStyle;
        private Style _selectedStyle;

        public bool IsNodeItem { get; set; }

        public List<TreeListItemModel> Children { get; set; }

        public TreeListItemModel ParentItem { get; set; }

        public int Level { get; set; }

        public Style HeaderStyle { get; set; }

        bool _isExpanded;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                IconSource = value ? "Arrow_down.png" : "Arrow_right.png";
                HeaderStyle = value ? _selectedStyle : _normalStyle;
                RaisePropertyChanged(() => IsExpanded);
                RaisePropertyChanged(() => IconSource);
                RaisePropertyChanged(() => HeaderStyle);
            }
        }

        public bool Search(string text)
        {
            bool contains = false;
            bool isChildrenContains = Children.Any(x => x.Header.Search(text));
            bool isHeaderContains = Header.Search(text);

            contains = isHeaderContains ||
                       isChildrenContains ||
                       (Children.Any(x => x.Search(text)));

            SetVisibility(contains, true);

            return contains;
        }

        public void SetVisibility(bool toVisible, bool isSearch = false)
        {
            if (toVisible)
            {
                IsExpanded = toVisible;

                var parentItem = ParentItem;

                while (parentItem != null)
                {
                    parentItem.IsExpanded = toVisible;
                    parentItem = parentItem.ParentItem;
                }
            }
            else
            {
                Children?.ForEach(y => y.IsExpanded = toVisible);
                IsExpanded = toVisible;
            }
        }

        public TreeListItemModel()
        {
            Children = new List<TreeListItemModel>();

            _normalStyle = (Style)Application.Current.Resources["TreeViewListEntryHeaderStyle"];
            _selectedStyle = (Style)Application.Current.Resources["TreeViewListEntryHeaderSelectedStyle"];

            HeaderStyle = _normalStyle;
        }
    }
}
