using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class RoundedLabelTemplate : Frame
    {
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public static readonly BindableProperty TextStyleProperty =
        BindableProperty.Create("TextStyle",
                               typeof(Style),
                               typeof(RoundedLabelTemplate),
                               null,
                               BindingMode.OneWay);


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
        BindableProperty.Create("Text",
                               typeof(string),
                               typeof(RoundedLabelTemplate),
                               string.Empty,
                               BindingMode.OneWay);


        public RoundedLabelTemplate()
        {
            InitializeComponent();

            PropertyChanged += RoundedLabelTemplate_PropertyChanged;
        }

        private void RoundedLabelTemplate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TextStyle))
            {
                this.CaptionLabel.Style = TextStyle;
            }
            else if (e.PropertyName == nameof(Text))
            {
                this.CaptionLabel.Text = Text;
            }
        }
    }
}
