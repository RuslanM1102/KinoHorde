using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopApplication.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для ColoredImage.xaml
    /// </summary>
    public partial class ColoredImage : UserControl
    {
        public Color ImageColor
        {
            get { return (Color)GetValue(ImageColorProperty); }
            set { SetValue(ImageColorProperty, value); }
        }

        public static readonly DependencyProperty ImageColorProperty =
            DependencyProperty.Register("ImageColor", typeof(Color), typeof(ColoredImage));

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
