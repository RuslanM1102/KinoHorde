using DesktopApplication.MVVM.View;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DesktopApplication.Core.Controls
{
    public class Extensions
    {
        public static readonly DependencyProperty ImageColorProperty =
            DependencyProperty.Register("ImageColor", typeof(Color), typeof(Extensions));
        public static Color GetImageColor(UIElement element)
        {
           return (Color)element.GetValue(ImageColorProperty); 
        }     
        public static void SetImageColor(UIElement element, Color value)
        {
            element.SetValue(ImageColorProperty, value); 
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(Extensions));

        public static Color GetImageSource(UIElement element)
        {
            return (Color)element.GetValue(ImageSourceProperty);
        }
        public static void SetImageSource(UIElement element, ImageSource value)
        {
            element.SetValue(ImageSourceProperty, value);
        }
    }
}
