using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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
            }
            catch (InvalidOperationException)
            {
                // Do nothing if there's no free space on the board in this column
                return;
            }
        }

        private void Check_Score()
        {
            // Set directions for checking progress
            (int dx, int dy)[] directions =
            [
                (1, 0), // horizontal
                (0, 1), // vertical
                (1, 1), // bottom-right
                (-1, 1) // bottom-left
            ];

            // Create matrix from the board
            int[,] board = new int[6, 7];
            string[] col_row;
            int col, row;

            // Add red pawns to matrix
            foreach (var pawn in GLOBALS.redpawns)
            {
                col_row = pawn.Name.Replace("circle", "").Split("_");
                col = Convert.ToInt32(col_row[1]);
                row = Convert.ToInt32(col_row[2]);

                board[row, col] = 1;
            }

            // Add yellow pawns to matrix
            foreach (var pawn in GLOBALS.yellowpawns)
            {
                col_row = pawn.Name.Replace("circle", "").Split("_");
                col = Convert.ToInt32(col_row[1]);
                row = Convert.ToInt32(col_row[2]);

                board[row, col] = 2;
            }

            // Check if it's draw
            if (GLOBALS.redpawns.Count + GLOBALS.yellowpawns.Count == 42) Draw();

            // Check every pawn in the matrix
            for (row = 0; row < 6; row++)
            {
                for (col = 0; col < 7; col++)
                {
                    // Skip elements with number 0
                    if (board[row, col] == 0) continue;

                    foreach (var (jump_column, jump_row) in directions)
                    {
                        Check_Four(board, row, col, jump_column, jump_row);
                    }
                }
            }

        }

        private void Check_Four(int[,] board, int initial_row, int initial_col, int jump_col, int jump_row)
        {
            List<(int row, int col)> selected = new();
            selected.Add((initial_row, initial_col));

            // Check 4 elements in every direction
            for (int i = 1; i < 4; i++)
            {
                int new_row = initial_row + jump_row * i;
                int new_col = initial_col + jump_col * i;

                if (new_row < 0 || new_row >= 6 || new_col < 0 || new_col >= 7)
                    return;

                if (board[new_row, new_col] != board[initial_row, initial_col])
                    return;

                selected.Add((new_row, new_col));
            }

            // This code runs when the game is won
            InfoLabel.Text = $"Player {GLOBALS.turn.ToUpper()} won";

            // Higlight the winning pawns
            foreach (var (row, col) in selected)
            {
                var pawn = Playground.Children.OfType<Ellipse>()
                    .Where(n => n.Name.StartsWith($"circle_{col}_{row}"))
                    .Last();

                pawn.Stroke = Brushes.GhostWhite;
            }

            // Turn off adding new pawns
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.IsEnabled = false;
            }
        }

        private void Draw()
        {
            // This code runs if there's not enough space on the board
            InfoLabel.Text = "It's a DRAW";

            // Turn off adding new pawns
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.IsEnabled = false;
            }
        }
    }
}
