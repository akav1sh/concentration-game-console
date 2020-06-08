using System;
using System.Text;
using System.Threading;
using Ex02.ConsoleUtils;

namespace B20_Ex02
{
    public class GameUI
    {
        private GameLogic m_GameLogic;

        public void RunGame()
        {
            Console.WriteLine("Welcome to the Concentration Game!{0}", Environment.NewLine);

            Player firstPlayer = new Player(getPlayerName("Please enter your name: "), Player.ePlayerType.Human);
            Player secondPlayer = getOpponent(out GameLogic.eGameMode gameMode);
            m_GameLogic = new GameLogic(firstPlayer, secondPlayer, gameMode);

            do
            {
                newGame();
            }
            while (isPlayerWantAnotherGame());

            Console.WriteLine("Press any key to exit . . .");
            Console.ReadKey();
        }

        private void newGame()
        {
            createNewBoard();

            do
            {
                displayGameInfo();

                string firstMove = executeMove("Please pick the first cell to expose", out bool toQuit);
                if (toQuit)
                {
                    break;
                }

                Console.WriteLine($"Chosen cell: {firstMove}");
                string secondMove = executeMove("Please pick the second cell to expose - to find a match", out toQuit);
                if (toQuit)
                {
                    break;
                }

                Console.WriteLine($"Chosen cell: {secondMove}");
                checkForMatch(firstMove, secondMove);
            }
            while (!m_GameLogic.IsGameOver());

            endGame();
        }

        private void checkForMatch(string i_FirstMove, string i_SecondMove)
        {
            if (m_GameLogic.CheckForMatch(i_FirstMove, i_SecondMove))
            {
                Console.WriteLine($"Point to {m_GameLogic.CurrentPlayer.Name}!!!");
            }

            Thread.Sleep(2000);
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

        private bool isPlayerWantAnotherGame()
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
                if (startAnotherGame)
                {
                    m_GameLogic.ResetGame();
                }
            }

            return startAnotherGame;
        }

        private string executeMove(string i_MsgToDisplay, out bool o_ToQuit)
        {
            string move;

            if (m_GameLogic.CurrentPlayer.PlayerType == Player.ePlayerType.Human)
            {
                move = getMoveFromPlayer(i_MsgToDisplay);
            }
            else
            {
                Thread.Sleep(1500);
                move = m_GameLogic.ChooseComputerMove();
            }

            o_ToQuit = m_GameLogic.GameStatus == GameLogic.eGameStatus.QuitGame;
            if (!o_ToQuit)
            {
                m_GameLogic.ToggleCellState(move);
                displayGameInfo();
            }

            return move;
        }

        private void displayGameInfo()
        {
            displayBoard();
            displayGameScore();

            if (m_GameLogic.GameStatus != GameLogic.eGameStatus.InProcess)
            {
                Console.WriteLine("Game over!");
            }
            else
            {
                displayCurrentPlayerName();
            }
        }

        private void endGame()
        {
            displayGameInfo();
            switch (m_GameLogic.GameStatus)
            {
                case GameLogic.eGameStatus.Win:
                    Console.WriteLine($"{m_GameLogic.GetWinner().Name} is the winner! Congratulations!");
                    break;
                case GameLogic.eGameStatus.Tie:
                    Console.WriteLine("It's a tie!");
                    break;
                default:
                    Console.WriteLine("Bye bye... See you next time!");
                    break;
            }
        }

        private Player getOpponent(out GameLogic.eGameMode o_GameMode)
        {
            Player opponent;

            o_GameMode = getGameModeFromPlayer();
            if (o_GameMode == GameLogic.eGameMode.PlayerVsPlayer)
            {
                opponent = new Player(getPlayerName("Please enter second player name: "), Player.ePlayerType.Human);
            }
            else
            {
                opponent = new Player("Computer", Player.ePlayerType.AI);
            }

            return opponent;
        }

        private void createNewBoard()
        {
            bool isValidBoardSize = false;

            while (!isValidBoardSize)
            {
                Console.WriteLine("Please enter the board size you prefer (minimum {0}x{0}, maximum {1}x{1}, even amount of cells):", GameBoard.MinHeightOrWidth, GameBoard.MaxHeightOrWidth);
                Console.Write("Height: ");
                string height = Console.ReadLine();
                Console.Write("Width: ");
                string width = Console.ReadLine();

                isValidBoardSize = m_GameLogic.IsValidBoardSize(height, width);
                if (isValidBoardSize)
                {
                    m_GameLogic.SetBoard(int.Parse(height), int.Parse(width));
                }
                else
                {
                    Console.WriteLine("Wrong board size entered!{0}", Environment.NewLine);
                }
            }
        }

        private void displayBoard()
        {
            StringBuilder boardToDisplay = new StringBuilder();
            string lineSeparator = makeLineSeparator();

            boardToDisplay.Append("  ");
            for (int i = 0; i < m_GameLogic.Board.Width; i++)
            {
                boardToDisplay.AppendFormat("  {0} ", (char)('A' + i));
            }

            boardToDisplay.Append(Environment.NewLine).Append(lineSeparator);
            for (int i = 0; i < m_GameLogic.Board.Height; i++)
            {
                boardToDisplay.AppendFormat("{0}{1} |", Environment.NewLine, i + 1);
                for (int j = 0; j < m_GameLogic.Board.Width; j++)
                {
                    string cell = m_GameLogic.IsCellVisible(i, j) ?
                        m_GameLogic.Board[i, j].ToString() : " ";

                    boardToDisplay.AppendFormat(" {0} |", cell);
                }

                boardToDisplay.Append(Environment.NewLine).Append(lineSeparator);
            }

            Screen.Clear();
            Console.WriteLine(boardToDisplay.ToString());
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

        private string getMoveFromPlayer(string i_MsgToDisplay)
        {
            bool isValidMove;
            string moveFromPlayer;

            do
            {
                Console.Write($"{i_MsgToDisplay} (for example: \"A1\") or press \"Q\" to exit: ");
                moveFromPlayer = Console.ReadLine().Trim();
                isValidMove = m_GameLogic.IsValidMove(moveFromPlayer, out GameLogic.ePlayerMoveStatus moveStatus);

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

            return moveFromPlayer;
        }

        private GameLogic.eGameMode getGameModeFromPlayer()
        {
            string msgToUser = string.Format(
@"Please select the game mode you prefer:
(1) Player Vs. Player
(2) Player Vs. Computer
Your selection: ");
            string gameMode;
            bool isValidGameMode = false;
            int parsedGameMode;

            do
            {
                Console.Write(msgToUser);
                gameMode = Console.ReadLine();

                if (int.TryParse(gameMode, out parsedGameMode) && Enum.IsDefined(typeof(GameLogic.eGameMode), parsedGameMode))
                {
                    isValidGameMode = true;
                }
                else
                {
                    Console.WriteLine("Wrong mode selected!{0}", Environment.NewLine);
                }
            }
            while (!isValidGameMode);

            return (GameLogic.eGameMode)parsedGameMode;
        }

        private string getPlayerName(string i_MsgToDisplay)
        {
            string playerName;

            do
            {
                Console.Write(i_MsgToDisplay);
                playerName = Console.ReadLine().TrimStart();
            }
            while (string.IsNullOrEmpty(playerName));

            return playerName;
        }
    }
}
