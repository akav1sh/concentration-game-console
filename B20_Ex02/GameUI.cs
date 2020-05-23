using System;
using System.Text;
using Ex02.ConsoleUtils;

namespace B20_Ex02
{
    public class GameUI
    {
        private GameLogic m_GameLogic;

        public void RunGame() // TODO
        {
            Player firstPlayer = new Player(getPlayerName("Please enter your name: "));
            Player opponent = getOpponent(out GameLogic.eGameMode gameMode);
            m_GameLogic = new GameLogic(firstPlayer, opponent, gameMode);
            createBoard();
            displayBoard(m_GameLogic.Board);
        }

        private Player getOpponent(out GameLogic.eGameMode io_GameMode)
        {
            Player opponent;
            io_GameMode = getGameModeFromUser();

            if(io_GameMode == GameLogic.eGameMode.PlayerVsPlayer)
            {
                opponent = new Player(getPlayerName("Please enter second player name: "));
            }
            else
            {
                opponent = new Player("Computer");
            }

            return opponent;
        }

        private void createBoard()
        {
            string requestMessageToUser = "Please enter the board size you prefer(Min 4x4, Max 6x6, even amount of cells): ";
            bool isValidBoardSize = false;
            int parsedHeight = 0, parsedWidth = 0;

            while (!isValidBoardSize)
            {
                Console.WriteLine(requestMessageToUser);
                Console.WriteLine("Height: ");
                string height = Console.ReadLine();
                Console.WriteLine("Width: ");
                string width = Console.ReadLine();

                isValidBoardSize = m_GameLogic.IsValidBoardSize(height, width);
                if (m_GameLogic.IsValidBoardSize(height, width))
                {
                    parsedHeight = int.Parse(height);
                    parsedWidth = int.Parse(width);
                }
                else
                {
                    Console.WriteLine("Wrong board size entered!");
                }
            }

            m_GameLogic.SetNewBoard(parsedHeight, parsedWidth);
        }
        
        private void displayBoard(GameBoard i_Board)
        {
            Screen.Clear();
            StringBuilder outBoard = new StringBuilder();
            string lineSeparator = makeLineSeparator(i_Board);
            outBoard.Append("  ");
            for(int i = 0; i < i_Board.Width; i++)
            {
                outBoard.AppendFormat("  {0} ", (char)('A' + i));
            }

            outBoard.Append(Environment.NewLine);
            outBoard.Append(lineSeparator);
            for (int i = 0; i < i_Board.Height; i++)
            {
                outBoard.AppendFormat("{0}{1} |", Environment.NewLine, i + 1);
                for(int j = 0; j < i_Board.Width; j++)
                {
                    string cell = " ";
                    if (i_Board.Board[i, j].Visible)
                    {
                        cell = i_Board.Board[i, j].Content.ToString();
                    }

                    outBoard.AppendFormat(" {0} |", cell);
                }

                outBoard.Append(Environment.NewLine);
                outBoard.Append(lineSeparator);
            }

            Console.WriteLine(outBoard.ToString());
        }

        private void getCellFromUser(out int o_Row, out int o_Column) // TODO
        {
            string requestMessageToUser = "Please enter wanted cell (for example: \"A4\"): ";
            Console.Write(requestMessageToUser);
            string receivedLine = Console.ReadLine();

            while(!m_GameLogic.IsValidCellInput(receivedLine, out GameLogic.eInputCellStatus cellStatus))
            {
                switch(cellStatus)
                {
                    case GameLogic.eInputCellStatus.InvalidCellFormat:
                        Console.WriteLine("Wrong format entered!");
                        break;
                    case GameLogic.eInputCellStatus.InvalidCellBorders:
                        Console.WriteLine("Cell outside of board entered!");
                        break;
                    case GameLogic.eInputCellStatus.VisibleCell:
                        Console.WriteLine("Cell outside of board entered!");
                        break;
                    default:
                        Console.WriteLine("Bye Bye cya next time!");
                        Environment.Exit(0);
                        break;
                }

                Console.Write(requestMessageToUser);
                receivedLine = Console.ReadLine();
            }

            o_Row = m_GameLogic.ExtractRow(receivedLine[1]);
            o_Column = m_GameLogic.ExtractColumn(receivedLine[0]);
        }

        private GameLogic.eGameMode getGameModeFromUser() // TODO
        {
            string requestMessageToUser = string.Format(
                @"
1. PlayerVsPlayer
2. PlayerVsComputer
Please enter the game mode you prefer: ");
            Console.Write(requestMessageToUser);
            string receivedLine = Console.ReadLine();

            while (!GameLogic.IsValidGameModeSelection(receivedLine))
            {
                Console.WriteLine("Wrong mode selected!");
                Console.Write(requestMessageToUser);
                receivedLine = Console.ReadLine();
            }

            return (GameLogic.eGameMode)int.Parse(receivedLine);
        }

        private string getPlayerName(string i_MessageToDisplay) // TODO
        {
            Console.Write(i_MessageToDisplay);
            string receivedLine = Console.ReadLine();

            while (!GameLogic.IsValidName(receivedLine))
            {
                Console.Write(i_MessageToDisplay);
                receivedLine = Console.ReadLine();
            }

            return receivedLine;
        }

        private string makeLineSeparator(GameBoard i_Board)
        {
            StringBuilder lineSeparator = new StringBuilder();

            lineSeparator.Append("  =");
            for(int i = 0; i < i_Board.Width; i++)
            {
                lineSeparator.Append("===");
                lineSeparator.Append("=");
            }

            return lineSeparator.ToString();
        }
    }
}
