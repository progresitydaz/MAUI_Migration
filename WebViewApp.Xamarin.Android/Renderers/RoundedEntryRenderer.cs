using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace WebViewApp.Xamarin.Droid.Renderers
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (RoundedEntry)Element;

                if (view.IsCurvedCornersEnabled)
                {
                    // creating gradient drawable for the curved background
                    var gradientBackground = new GradientDrawable();
                    gradientBackground.SetShape(ShapeType.Rectangle);
                    gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                    // Thickness of the stroke line
                    gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                    // Radius for the curves
                    gradientBackground.SetCornerRadius(
                        RenderUtil.DpToPixels(this.Context,
                            Convert.ToSingle(view.CornerRadius)));

                    // set the background of the label
                    Control.SetBackground(gradientBackground);
                }

                // Set padding for the internal text from border
                Control.SetPadding(
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(10)),
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(0)),
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(10)),
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(0)));

                Control.Gravity = GravityFlags.CenterVertical;

                if (this.Element.Keyboard == Keyboard.Numeric)
                {
                    RenderUtil.AddDoneAndNextButton(Control, view);
                }
            }
        }
    }
}