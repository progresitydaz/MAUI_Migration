using System;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            Layer.CornerRadius = 10;
            Layer.BorderColor = Color.FromHex("d8d8d8").ToCGColor();
            Layer.BorderWidth = (float)1.0;
        }
    }
}
