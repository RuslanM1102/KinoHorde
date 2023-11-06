using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopApplication.Core.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is Color color)
                return new SolidColorBrush(color);
            throw new InvalidOperationException($"Unsupported type {value.GetType().Name}, ColorToSolidColorBrushValueConverter.Convert()");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is SolidColorBrush solid)
                return solid.Color;

            throw new InvalidOperationException($"Unsupported type {value.GetType().Name}, ColorToSolidColorBrushValueConverter.ConvertBack()");
        }
    }
}
