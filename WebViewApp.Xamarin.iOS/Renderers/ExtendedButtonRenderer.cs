using System;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class ExtendedButtonRenderer : ButtonRenderer
    {
        public ExtendedButtonRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    var view = (ExtendedButton)Element;

                    Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);

                    Control.Layer.BorderWidth = (nfloat)0.0;
                }
            }
        }
    }
}
