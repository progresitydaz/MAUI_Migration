using System;
using Android.Content;
using Android.Graphics.Drawables;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace WebViewApp.Xamarin.Droid.Renderers
{
    public class ExtendedButtonRenderer :ButtonRenderer
    {
        public ExtendedButtonRenderer(Context context)
           : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (ExtendedButton)Element;

                // creating gradient drawable for the curved background
                var gradientBackground = new GradientDrawable();
                gradientBackground.SetShape(ShapeType.Rectangle);
                gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                // Thickness of the stroke line
                gradientBackground.SetStroke(0, view.BackgroundColor.ToAndroid());

                // Radius for the curves
                gradientBackground.SetCornerRadius(
                    RenderUtil.DpToPixels(this.Context,
                        Convert.ToSingle(view.CornerRadius)));

                // set the background of the label
                Control.SetBackground(gradientBackground);
            }
        }
    }
}
