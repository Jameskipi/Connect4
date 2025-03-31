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

namespace Connect4
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

        private void Rectangle_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                rectangle.Fill = Brushes.Aqua;
            }
        }

        private void Rectangle_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                rectangle.Fill = Brushes.Black;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.AddHandler(Rectangle.MouseEnterEvent, new RoutedEventHandler(Rectangle_MouseEnter));
                rect.AddHandler(Rectangle.MouseLeaveEvent, new RoutedEventHandler(Rectangle_MouseLeave));
            }
        }
    }
}