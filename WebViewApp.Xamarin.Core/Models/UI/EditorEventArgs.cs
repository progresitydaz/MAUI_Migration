using System;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class EditorEventArgs
    {
        public string Tag { get; set; }

        public string Text { get; set; }

        public bool IsFocused { get; set; }

        public Element ScrollElement { get; set; }
    }
}
