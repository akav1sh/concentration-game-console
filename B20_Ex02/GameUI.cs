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
            Console.WriteLine("Welcome to the Concentration Game!{0}", Environment.NewLine);
            Player firstPlayer = new Player(getPlayerName("Please enter your name: "));
            Player secondPlayer = getOpponent(out GameLogic.eGameMode gameMode);
            m_GameLogic = new GameLogic(firstPlayer, secondPlayer, gameMode);

            createNewBoard();
            displayBoard();
        }

        private Player getOpponent(out GameLogic.eGameMode io_GameMode)
        {
            Player opponent;
            io_GameMode = getGameModeFromUser();
            
            if (io_GameMode == GameLogic.eGameMode.PlayerVsPlayer)
            {
                opponent = new Player(getPlayerName("Please enter second player name: "));
            }
            else
            {
                opponent = new Player("Computer");
            }

            return opponent;
        }

        private void createNewBoard()
        {
            string msgToUser = "Please enter the board size you prefer (minimum 4x4, maximum 6x6, even amount of cells):";
            bool isValidBoardSize = false;
            int parsedHeight = 0, parsedWidth = 0;

            while (!isValidBoardSize)
            {
                Console.WriteLine(msgToUser);
                Console.Write("Height: ");
                string height = Console.ReadLine();
                Console.Write("Width: ");
                string width = Console.ReadLine();

                isValidBoardSize = m_GameLogic.IsValidBoardSize(height, width);
                if (m_GameLogic.IsValidBoardSize(height, width))
                {
                    parsedHeight = int.Parse(height);
                    parsedWidth = int.Parse(width);
                }
                else
                {
                    Console.WriteLine("Wrong board size entered!{0}", Environment.NewLine);
                }
            }

            m_GameLogic.SetBoard(parsedHeight, parsedWidth);
        }
        
        private void displayBoard()
        {
            GameBoard gameBoard = m_GameLogic.Board;
            StringBuilder boardToDisplay = new StringBuilder();
            string lineSeparator = makeLineSeparator(gameBoard);

            boardToDisplay.Append("  ");
            for (int i = 0; i < gameBoard.Width; i++)
            {
                boardToDisplay.AppendFormat("  {0} ", (char)('A' + i));
            }

            boardToDisplay.Append(Environment.NewLine).Append(lineSeparator);
            for (int i = 0; i < gameBoard.Height; i++)
            {
                boardToDisplay.AppendFormat("{0}{1} |", Environment.NewLine, i + 1);
                for (int j = 0; j < gameBoard.Width; j++)
                {
                    string cell = " ";
                    if (gameBoard.Board[i, j].IsVisible)
                    {
                        cell = gameBoard[i, j].ToString();
                    }

                    boardToDisplay.AppendFormat(" {0} |", cell);
                }

                boardToDisplay.Append(Environment.NewLine).Append(lineSeparator);
            }

            Screen.Clear();
            Console.WriteLine(boardToDisplay.ToString());
        }

        private void getCellFromUser(out int o_Row, out int o_Column)
        {
            string msgToUser = "Please enter wanted cell (for example: \"A4\"): ";
            Console.Write(msgToUser);
            string userInput = Console.ReadLine();

            while(!m_GameLogic.IsValidInputCell(userInput, out GameLogic.eInputCellStatus cellStatus))
            {
                switch (cellStatus)
                {
                    case GameLogic.eInputCellStatus.InvalidCellFormat:
                        Console.WriteLine("Wrong format entered!{0}", Environment.NewLine);
                        break;
                    case GameLogic.eInputCellStatus.InvalidCellBounds:
                        Console.WriteLine("Cell outside of board bounds entered!{0}", Environment.NewLine);
                        break;
                    case GameLogic.eInputCellStatus.VisibleCell:
                        Console.WriteLine("Cell outside of board entered!{0}", Environment.NewLine);
                        break;
                    default:
                        Console.WriteLine("Bye Bye... see you next time!");
                        Environment.Exit(0);
                        break;
                }

                Console.Write(msgToUser);
                userInput = Console.ReadLine();
            }

            o_Column = m_GameLogic.ExtractColumn(userInput[0]);
            o_Row = m_GameLogic.ExtractRow(userInput[1]);
        }

        private GameLogic.eGameMode getGameModeFromUser()
        {
            string msgToUser = string.Format(
@"Please select the game mode you prefer:
(1) Player Vs. Player
(2) Player Vs. Computer
Your selection: ");
            Console.Write(msgToUser);
            string userInput = Console.ReadLine();

            while (!GameLogic.IsValidGameModeSelection(userInput))
            {
                Console.WriteLine("Wrong mode selected!{0}", Environment.NewLine);
                Console.Write(msgToUser);
                userInput = Console.ReadLine();
            }

            return (GameLogic.eGameMode)int.Parse(userInput);
        }

        private string getPlayerName(string i_MsgToDisplay)
        {
            Console.Write(i_MsgToDisplay);
            string userInput = Console.ReadLine();

            while (!GameLogic.IsValidName(userInput))
            {
                Console.Write(i_MsgToDisplay);
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        private string makeLineSeparator(GameBoard i_Board)
        {
            StringBuilder lineSeparator = new StringBuilder();

            lineSeparator.Append("  =");
            for (int i = 0; i < i_Board.Width; i++)
            {
                lineSeparator.Append("===");
                lineSeparator.Append("=");
            }

            return lineSeparator.ToString();
        }
    }
}
