using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ConnectFour
{
    public partial class MainWindow : Window
    {
        private void Red_Turn(string column_number)
        {
            try
            {
                // Get last free space in this column
                var pawn = Playground.Children.OfType<Ellipse>()
                    .Where(n => n.Name.StartsWith($"circle_{column_number}"))
                    .Except(GLOBALS.redpawns.OfType<Ellipse>())
                    .Except(GLOBALS.yellowpawns.OfType<Ellipse>())
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
                    .Except(GLOBALS.yellowpawns.OfType<Ellipse>())
                    .ToArray()
                    .Last();


                // Add new pawn
                GLOBALS.yellowpawns.Add(pawn);
                pawn.Fill = Brushes.Gold;
                TestLabel.Content = pawn.Name;
            }
            catch (InvalidOperationException)
            {
                // Do nothing if there's no free space on the board in this column
                return;
            }
        }
    }
}
