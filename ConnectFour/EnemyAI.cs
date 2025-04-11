using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConnectFour
{
    public partial class MainWindow : Window
    {
        private void AI_Turn(object sender)
        {
            switch (GLOBALS.enemystatus)
            {
                case "random":
                    AI_Random();
                    break;

                case "aggressive":
                    AI_Aggressive(); 
                    break;
            }

            Check_Score();
            GLOBALS.turn = "red";
        }

        private void AI_Random()
        {
            Random random = new Random();
            string random_column = random.Next(0, 7).ToString();

            Yellow_Turn(random_column);
        }

        private void AI_Aggressive()
        {
            Yellow_Turn("0");
        }
    }
}
