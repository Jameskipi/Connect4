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

        private void Add_Circles()
        {
            // Create circles on the board

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

        private void Red_Turn(string column_number) 
        {
            try
            {
                // Get last free space in this column
                var pawn = Playground.Children.OfType<Ellipse>()
                    .Where(n => n.Name.StartsWith($"circle_{column_number}"))
                    .Except(GLOBALS.redpawns.OfType<Ellipse>())
                    .ToArray()
                    .Last();


                // Add new pawn
                GLOBALS.redpawns.Add(pawn);
                pawn.Fill = Brushes.Red;
                TestLabel.Content = pawn.Name;
            }
            catch (InvalidOperationException)
            {
                // Do nothing if there's no free space on the board in this column
                return;
            }
        }

        private void Yellow_Turn(string column_number)
        {
            try
            {
                // Get last free space in this column
                var pawn = Playground.Children.OfType<Ellipse>()
                    .Where(n => n.Name.StartsWith($"circle_{column_number}"))
                    .Except(GLOBALS.redpawns.OfType<Ellipse>())
                    .ToArray()
                    .Last();


                // Add new pawn
                GLOBALS.redpawns.Add(pawn);
                pawn.Fill = Brushes.Gold;
                TestLabel.Content = pawn.Name;
            }
            catch (InvalidOperationException)
            {
                // Do nothing if there's no free space on the board in this column
                return;
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

            string column_number = rectangle.Name.Replace("r", "");

            if (GLOBALS.turn == "red")
            {
                Red_Turn(column_number);
                GLOBALS.turn = "yellow";
            }
            else
            {
                Yellow_Turn(column_number);
                GLOBALS.turn = "red";
            }
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
    }
}