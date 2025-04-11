using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
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
                // If it's ai turn try again
                if (GLOBALS.enemyai)
                {
                    AI_Random();
                }

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

            int new_row = 0, new_col = 0;

            // Check 4 elements in given direction
            for (int i = 1; i < 4; i++)
            {
                new_row = initial_row + jump_row * i;
                new_col = initial_col + jump_col * i;

                if (new_row < 0 || new_row >= 6 || new_col < 0 || new_col >= 7)
                    break;

                if (board[new_row, new_col] != board[initial_row, initial_col])
                    break;

                selected.Add((new_row, new_col));
            }

            switch (selected.Count)
            {
                case 1:
                    return;
                case 2:
                    return;
                case 3:
                    // Close to winning
                    bool next_safe = false;
                    bool before_safe = false;
                    int next_row = new_row + jump_row;
                    int next_col = new_col + jump_col;
                    int before_row = initial_row - jump_row;
                    int before_col = initial_col - jump_col;

                    // Check next possibility
                    try
                    {
                        // Fix bug for horizontal prediciton
                        if (jump_row == 0 && jump_col == 1) next_col -= 1;

                        if (board[next_row, next_col] != 0)
                        {
                            next_safe = true;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        next_safe = true;
                    }

                    // Check before possibility
                    try 
                    {
                        if (board[before_row, before_col] != 0)
                        {
                            before_safe = true;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        before_safe = true;
                    }

                    // Decide if AI should be aggressive
                    if (next_safe && before_safe)
                    {
                        return;
                    }
                    else if (!next_safe)
                    {
                        GLOBALS.aggressivemove = next_col;
                        GLOBALS.enemystatus = "aggressive";
                    }
                    else if (!before_safe)
                    {
                        GLOBALS.aggressivemove = before_col;
                        GLOBALS.enemystatus = "aggressive";
                    }
                    else
                    {
                        GLOBALS.enemystatus = "aggressive";
                    }

                    return;

                case 4:
                    // Game won
                    InfoLabel.Text = $"Player {GLOBALS.turn.ToUpper()} won";
                    GLOBALS.gamestatus = false;
                    break;
            }

            // Code below runs when the game ends

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
            GLOBALS.gamestatus = false;

            // Turn off adding new pawns
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.IsEnabled = false;
            }
        }

        private void Reset()
        {
            // Reset global variables
            GLOBALS.redpawns = [];
            GLOBALS.yellowpawns = [];
            GLOBALS.turn = "red";
            GLOBALS.gamestatus = true;
            GLOBALS.enemystatus = "random";

            InfoLabel.Text = $"It's {GLOBALS.turn} turn";

            // Turn on adding new pawns
            foreach (Rectangle rect in Rectangles.Children)
            {
                rect.IsEnabled = true;
            }

            // Clear the board
            var circles = Playground.Children.OfType<Ellipse>()
                    .Where(n => n.Name.StartsWith($"circle_"));

            foreach (var circle in circles)
            {
                circle.Fill = Brushes.White;
                circle.Stroke = Brushes.Blue;
            }
        }
    }
}
