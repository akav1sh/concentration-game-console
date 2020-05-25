using System;
using System.Text;
using System.Threading;
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
            do
            {
                startNewGame();
            }
            while (playerWantAnotherGame());

            Console.WriteLine("Press any key to exit . . .");
            Console.ReadKey();
        }

        private void startNewGame() // TODO
        {
            bool isGameOver = false;
            string firstMove, secondMove;

            createNewBoard();
            while (!isGameOver)
            {
                displayBoard();
                displayGameScore();
                displayCurrentPlayerName();

                firstMove = getMoveFromPlayer("Please pick the first cell to expose");
                if (m_GameLogic.GameStatus == GameLogic.eGameStatus.QuitGame)
                {
                    break;
                }

                m_GameLogic.ToggleCellState(firstMove);
                displayBoard();
                
                secondMove = getMoveFromPlayer("Please pick the second cell to expose - to find a match");
                if (m_GameLogic.GameStatus == GameLogic.eGameStatus.QuitGame)
                {
                    break;
                }

                checkForMatch(firstMove, secondMove);

                isGameOver = m_GameLogic.IsGameOver();
            }

            endGame();
        }

        private void displayGameScore()
        {
            string scoreMsg = string.Format(
@"{0}'s score: {1}
{2}'s score: {3}{4}",
m_GameLogic.Player1.Name,
m_GameLogic.Player1.Score,
m_GameLogic.Player2.Name,
m_GameLogic.Player2.Score,
Environment.NewLine);
            Console.WriteLine(scoreMsg);
        }

        private void displayCurrentPlayerName()
        {
            Console.WriteLine($"It's {m_GameLogic.CurrentPlayer.Name}'s turn!");
        }

        private void checkForMatch(string i_FirstMove, string i_SecondMove)
        {
            m_GameLogic.ToggleCellState(i_SecondMove);
            displayBoard();
            if (!m_GameLogic.IsMatch(i_FirstMove, i_SecondMove))
            {
                Thread.Sleep(2000);
                m_GameLogic.ToggleCellState(i_FirstMove);
                m_GameLogic.ToggleCellState(i_SecondMove);
                m_GameLogic.TogglePlayer();
            }
            else
            {
                m_GameLogic.HiddenPairCellsAmount--;
                m_GameLogic.CurrentPlayer.Score++;
            }
        }

        private bool playerWantAnotherGame() // TODO
        {
            bool startAnotherGame = false;
            ConsoleKey response;

            if (m_GameLogic.GameStatus != GameLogic.eGameStatus.QuitGame)
            {
                do
                {
                    Console.Write("Would you like to play another game? (y/n): ");
                    response = Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                }
                while (response != ConsoleKey.Y && response != ConsoleKey.N);

                startAnotherGame = response == ConsoleKey.Y;
                m_GameLogic.ResetGame();
            }

            return startAnotherGame;
        }

        private void executeComputerMove() // TODO
        {
        }

        private void endGame()
        {
            Console.WriteLine("Game over!");
            switch (m_GameLogic.GameStatus)
            {
                case GameLogic.eGameStatus.Win:
                    Player winner = m_GameLogic.GetWinner();
                    Console.WriteLine($"{winner.Name} is the winner! Congratulations!");
                    break;
                case GameLogic.eGameStatus.Tie:
                    Console.WriteLine("It's a tie!");
                    break;
                default:
                    Console.WriteLine($"Bye bye {m_GameLogic.CurrentPlayer.Name}... See you next time!");
                    break;
            }
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
            bool isValidBoardSize = false;
            int parsedHeight = 0, parsedWidth = 0;

            while (!isValidBoardSize)
            {
                Console.WriteLine("Please enter the board size you prefer (minimum 4x4, maximum 6x6, even amount of cells):");
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
            string lineSeparator = makeLineSeparator();

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
                    if (gameBoard.Board[i, j].Visible)
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

        private string getMoveFromPlayer(string i_MsgToDisplay)
        {
            bool isValidMove;
            string userInput;

            do
            {
                Console.Write($"{i_MsgToDisplay} (for example: \"A4\") or press \"Q\" to exit: ");
                userInput = Console.ReadLine();
                isValidMove = m_GameLogic.IsValidMove(userInput, out GameLogic.ePlayerMoveStatus moveStatus);

                switch (moveStatus)
                {
                    case GameLogic.ePlayerMoveStatus.InvalidCellFormat:
                        Console.WriteLine("Wrong cell format entered!{0}", Environment.NewLine);
                        break;
                    case GameLogic.ePlayerMoveStatus.InvalidCellBounds:
                        Console.WriteLine("Cell outside of board bounds entered!{0}", Environment.NewLine);
                        break;
                    case GameLogic.ePlayerMoveStatus.VisibleCell:
                        Console.WriteLine("Cell already exposed!{0}", Environment.NewLine);
                        break;
                    case GameLogic.ePlayerMoveStatus.QuitGame:
                        m_GameLogic.GameStatus = GameLogic.eGameStatus.QuitGame;
                        break;
                }
            }
            while (!isValidMove);

            return userInput;
        }

        private GameLogic.eGameMode getGameModeFromUser()
        {
            string msgToUser = string.Format(
@"Please select the game mode you prefer:
(1) Player Vs. Player
(2) Player Vs. Computer
Your selection: ");
            string userInput;
            bool isValidGameMode;

            do
            {
                Console.Write(msgToUser);
                userInput = Console.ReadLine();
                isValidGameMode = GameLogic.IsValidGameModeSelection(userInput);
                if (!isValidGameMode)
                {
                    Console.WriteLine("Wrong mode selected!{0}", Environment.NewLine);
                }
            }
            while (!isValidGameMode);

            return (GameLogic.eGameMode)int.Parse(userInput);
        }

        private string getPlayerName(string i_MsgToDisplay)
        {
            string userInput;
            bool isValidName;

            do
            {
                Console.Write(i_MsgToDisplay);
                userInput = Console.ReadLine();
                isValidName = GameLogic.IsValidName(userInput);
            }
            while (!isValidName);

            return userInput;
        }

        private string makeLineSeparator()
        {
            StringBuilder lineSeparator = new StringBuilder();

            lineSeparator.Append("  =");
            for (int i = 0; i < m_GameLogic.Board.Width; i++)
            {
                lineSeparator.Append("====");
            }

            return lineSeparator.ToString();
        }
    }
}
