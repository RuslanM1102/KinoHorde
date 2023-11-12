using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopApplication.Core.Controls
{
    /// <summary>
    /// Логика взаимодействия для ColoredImage.xaml
    /// </summary>
    public partial class ColoredImage : UserControl
    {
        public SolidColorBrush ImageColor
        {
            get { return (SolidColorBrush)GetValue(ImageColorProperty); }
            set { SetValue(ImageColorProperty, value); }
        }

        public static readonly DependencyProperty ImageColorProperty =
            DependencyProperty.Register("ImageColor", typeof(SolidColorBrush), typeof(ColoredImage));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ColoredImage));

        public ColoredImage()
        {
            InitializeComponent();
        }
    }
}
