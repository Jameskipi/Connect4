using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnectFour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Circles()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    var circle = new Ellipse
                    {
                        Name = $"circle_{i}_{j}",
                        Height = 75,
                        Width = 75,
                        Fill = Brushes.White,
                        StrokeThickness = 5,
                        Stroke = Brushes.Blue
                    };

                    Canvas.SetLeft(circle, 12.5 + (100 * i));
                    Canvas.SetTop(circle, 30 + (100 * j));

                    Playground.Children.Add(circle);
                }
            }
        }

        private void Rectangle_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                rectangle.Fill = Brushes.Black;
                rectangle.Opacity = 0.1;
            }
        }

        private void Rectangle_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                rectangle.Opacity = 0;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.AddHandler(Rectangle.MouseEnterEvent, new RoutedEventHandler(Rectangle_MouseEnter));
                rect.AddHandler(Rectangle.MouseLeaveEvent, new RoutedEventHandler(Rectangle_MouseLeave));
            }

            Add_Circles();
        }
    }
}