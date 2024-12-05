using System;
using CoreGraphics;
using WebViewApp.Xamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(ExtendedDatePickerRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (DatePicker)Element;

                Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                Control.LeftViewMode = UITextFieldViewMode.Always;

                Control.Layer.CornerRadius = 0f;
                Control.Layer.BorderColor = Color.FromHex("#8f9fad").ToCGColor();
                Control.Layer.BorderWidth = 1f;
                Control.ClipsToBounds = true;
            }
        }
    }
}
