using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebViewApp.Xamarin.Core.Models
{
    public class HomeMenuGroup : ObservableCollection<HomeMenuItem>
    {
        public string Title { get; private set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }

        public HomeMenuGroup(string title)
            : base()
        {
            Title = title;
        }

        public HomeMenuGroup(string title, IEnumerable<HomeMenuItem> source)
            : base(source)
        {
            Title = title;
        }
    }
}