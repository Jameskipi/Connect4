using System.Reflection;
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
        public static class GLOBALS
        {
            public static List<Ellipse> redpawns = [];
            public static List<Ellipse> yellowpawns = [];
            public static string turn = "red";
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.AddHandler(Rectangle.MouseEnterEvent, new RoutedEventHandler(Rectangle_MouseEnter));
                rect.AddHandler(Rectangle.MouseLeaveEvent, new RoutedEventHandler(Rectangle_MouseLeave));
                rect.AddHandler(Rectangle.MouseDownEvent, new RoutedEventHandler(Rectangle_MouseDown));
            }

            Add_Circles();
        }

        private void Add_Circles()
        {
            // Create circles on the board

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    var circle = new Ellipse
                    {
                        Name = $"circle_{i}_{j}",
                        Height = 50,
                        Width = 50,
                        Fill = Brushes.White,
                        StrokeThickness = 5,
                        Stroke = Brushes.Blue
                    };

                    Canvas.SetLeft(circle, 10 + (70 * i));
                    Canvas.SetTop(circle, 10 + (70 * j));

                    Playground.Children.Add(circle);
                }
            }
        }

        private void Rectangle_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            rectangle.Fill = Brushes.Black;
            rectangle.Opacity = 0.1;
        }

        private void Rectangle_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }
            
            rectangle.Opacity = 0;
        }

        private void Rectangle_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            InfoLabel.Text = $"It's {GLOBALS.turn} turn";

            string column_number = rectangle.Name.Replace("r", "");

            if (GLOBALS.turn == "red")
            {
                Red_Turn(column_number);
                Check_Score();
                GLOBALS.turn = "yellow";
            }
            else
            {
                Yellow_Turn(column_number);
                Check_Score();
                GLOBALS.turn = "red";
            }
        }
    }
}