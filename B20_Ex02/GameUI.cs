using System;
using System.Text;

namespace B20_Ex02
{
    public class GameUI
    {
        public void RunGame() // TODO
        {
            GameBoard board = new GameBoard(6, 6);
            DisplayBoard(board);
        }

        public void DisplayBoard(GameBoard i_Board)
        {
            StringBuilder outBoard = new StringBuilder();
            string lineSeparator = makeLineSeparator(i_Board);
            outBoard.Append("  ");
            for(int i = 0; i < i_Board.Width; i++)
            {
                outBoard.AppendFormat("  {0} ", 'A' + i);
            }

            outBoard.AppendLine(lineSeparator);
            for (int i = 0; i < i_Board.Height; i++)
            {
                outBoard.AppendFormat("{0} |", i + 1);
                for(int j = 0; j < i_Board.Width; j++)
                {
                    string cell = " ";
                    if (i_Board.Board[i, j].Visible)
                    {
                        cell = i_Board.Board[i, j].Content.ToString();
                    }

                    outBoard.AppendFormat(" {0} |{1}", cell, Environment.NewLine);
                }
                outBoard.AppendLine(lineSeparator);
            }
        }

        public string GetCellFromUser() // TODO
        {
            Console.Write("Please enter wanted cell (for example: \"A4\"): ");
            return Console.ReadLine();
        }

        public string GetGameModeFromUser() // TODO
        {

            Console.Write("Please enter the game mode you prefer: ");
            return Console.ReadLine();
        }

        public string GetPlayerName(int i_PlayerNumber = 1) // TODO
        {
            Console.Write($"Please enter the name of player {i_PlayerNumber}: ");
            return Console.ReadLine();
        }

        private string makeLineSeparator(GameBoard i_Board)
        {
            StringBuilder lineSeparator = new StringBuilder();
            lineSeparator.Append("=");
            for(int i = 0; i < i_Board.Width; i++)
            {
                lineSeparator.Append("===");
            }
            lineSeparator.Append("=");
            return lineSeparator.ToString();
        }
    }
}
