using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Controls
{
    public class RoundedEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor),
                typeof(Color), typeof(RoundedEntry), Color.Gray);
        
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int),
                typeof(RoundedEntry), Device.OnPlatform<int>(1, 2, 2));
        
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius),
                typeof(double), typeof(RoundedEntry), Device.OnPlatform<double>(0, 0, 0));
        
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
            BindableProperty.Create(nameof(IsCurvedCornersEnabled),
                typeof(bool), typeof(RoundedEntry), true);
        
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public static readonly BindableProperty NextTextFieldProperty =
            BindableProperty.Create(nameof(NextTextField),
                                    typeof(Entry), typeof(RoundedEntry), null);

        public Entry NextTextField
        {
            get => (Entry)GetValue(NextTextFieldProperty);
            set => SetValue(NextTextFieldProperty, value);
        }

    }
}