using System;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class SelectionMenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }

        public Style Style { get; set; }

        public SelectionMenuItem()
        {
        }
    }
}
