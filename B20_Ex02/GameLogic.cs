using System;
using System.Text;

namespace B20_Ex02
{
    public class GameLogic
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly eGameMode r_GameMode;
        private eGameStatus m_GameStatus;
        private Player m_CurrentPlayer;
        private GameBoard m_Board;
        private int m_HiddenPairCellsAmount;
        internal static readonly Random sr_RandGenerator = new Random();
        
        public GameLogic(Player i_Player1, Player i_Player2, eGameMode i_GameGameMode)
        {
            r_Player1 = i_Player1;
            r_Player2 = i_Player2;
            r_GameMode = i_GameGameMode;
            m_GameStatus = eGameStatus.InProcess;
            m_CurrentPlayer = i_Player1;
            m_Board = null;
            m_HiddenPairCellsAmount = 0;
        }

        public enum eGameMode
        {
            PlayerVsPlayer = 1,
            PlayerVsComputer = 2
        }

        public enum eGameStatus
        {
            InProcess,
            Win,
            Tie,
            QuitGame
        }

        public enum ePlayerMoveStatus
        {
            InvalidCellFormat,
            InvalidCellBounds,
            VisibleCell,
            QuitGame,
            ValidCell
        }

        public GameBoard Board
        {
            get
            {
                return m_Board;
            }
        }

        public eGameMode GameMode
        {
            get
            {
                return r_GameMode;
            }
        }

        public eGameStatus GameStatus
        {
            get
            {
                return m_GameStatus;
            }

            set
            {
                m_GameStatus = value;
            }
        }

        public Player Player1
        {
            get
            {
                return r_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return r_Player2;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
            }
        }

        public int HiddenPairCellsAmount
        {
            get
            {
                return m_HiddenPairCellsAmount;
            }
        }

        public static bool IsValidName(string i_NameToCheck)
        {
            return string.IsNullOrEmpty(i_NameToCheck) == false;
        }

        public static bool IsValidGameModeSelection(string i_GameMode)
        {
            return int.TryParse(i_GameMode, out int mode) && ((eGameMode)mode == eGameMode.PlayerVsPlayer || (eGameMode)mode == eGameMode.PlayerVsComputer);
        }

        public bool IsCellVisible(int i_Row, int i_Column)
        {
            return m_Board.Board[i_Row, i_Column].Visible;
        }

        private bool areAllCellsVisible()
        {
            return m_HiddenPairCellsAmount == 0;
        }

        public void SetBoard(int i_Height, int i_Width)
        {
            m_Board = new GameBoard(i_Height, i_Width);
            m_HiddenPairCellsAmount = (i_Height * i_Width) / 2;
        }

        public bool IsValidMove(string i_CellToCheck, out ePlayerMoveStatus o_Status)
        {
            bool isValidMove;

            if (i_CellToCheck == "Q")
            {
                isValidMove = true;
                o_Status = ePlayerMoveStatus.QuitGame;
            }
            else
            {
                if (i_CellToCheck.Length != 2 || !char.IsUpper(i_CellToCheck[0]) || 
                    !char.IsDigit(i_CellToCheck[1]))
                {
                    isValidMove = false;
                    o_Status = ePlayerMoveStatus.InvalidCellFormat;
                }
                else
                {
                    int column = convertColumnCharToInt(i_CellToCheck[0]);
                    int row = convertRowCharToInt(i_CellToCheck[1]);
                    if (column < 0 || column > m_Board.Width - 1 || row < 0 || row > m_Board.Height - 1)
                    {
                        isValidMove = false;
                        o_Status = ePlayerMoveStatus.InvalidCellBounds;
                    }
                    else
                    {
                        if (m_Board.Board[row, column].Visible)
                        {
                            isValidMove = false;
                            o_Status = ePlayerMoveStatus.VisibleCell;
                        }
                        else
                        {
                            isValidMove = true;
                            o_Status = ePlayerMoveStatus.ValidCell;
                        }
                    }
                }
            }

            return isValidMove;
        }

        public bool IsValidBoardSize(string i_Height, string i_Width)
        {
            bool isValidBoardSize;

            if (!int.TryParse(i_Height, out int height) || !int.TryParse(i_Width, out int width))
            {
                isValidBoardSize = false;
            }
            else
            {
                if (height < GameBoard.k_MinHeightOrWidth || height > GameBoard.k_MaxHeightOrWidth ||
                    width < GameBoard.k_MinHeightOrWidth || width > GameBoard.k_MaxHeightOrWidth)
                {
                    isValidBoardSize = false;
                }
                else
                {
                    isValidBoardSize = (height * width) % 2 == 0;
                }
            }

            return isValidBoardSize;
        }

        private int convertColumnCharToInt(char i_Column)
        {
            return i_Column - 'A';
        }

        private int convertRowCharToInt(char i_Row)
        {
            return int.Parse(i_Row.ToString()) - 1;
        }

        private char convertColumnIntToChar(int i_Column)
        {
            return (char)('A' + i_Column);
        }

        private char convertRowIntToChar(int i_Row)
        {
            return char.Parse((i_Row + 1).ToString());
        }

        public void ToggleCellState(string i_CellToToggle)
        {
            int column = convertColumnCharToInt(i_CellToToggle[0]);
            int row = convertRowCharToInt(i_CellToToggle[1]);
            bool updatedCellState = !m_Board.Board[row, column].Visible;

            m_Board.SetCellState(row, column, updatedCellState);
        }

        public bool CheckForMatch(string i_FirstMove, string i_SecondMove)
        {
            bool isMatch = this.isMatch(i_FirstMove, i_SecondMove);

            if (isMatch)
            {
                m_HiddenPairCellsAmount--;
                m_CurrentPlayer.Score++;
            }
            else
            {
                ToggleCellState(i_FirstMove);
                ToggleCellState(i_SecondMove);
                togglePlayer();
            }

            return isMatch;
        }

        private bool isMatch(string i_FirstCell, string i_SecondCell)
        {
            return m_Board[convertRowCharToInt(i_FirstCell[1]), convertColumnCharToInt(i_FirstCell[0])] == m_Board[convertRowCharToInt(i_SecondCell[1]), convertColumnCharToInt(i_SecondCell[0])];
        }

        private void togglePlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public void ResetGame()
        {
            m_CurrentPlayer = GetWinner();
            Player1.Score = 0;
            Player2.Score = 0;
            m_GameStatus = eGameStatus.InProcess;
        }

        public string ChooseComputerMove()
        {
            int randomRow, randomColumn;
            StringBuilder computerMove = new StringBuilder();

            while (true)
            {
                randomRow = sr_RandGenerator.Next(0, m_Board.Height);
                randomColumn = sr_RandGenerator.Next(0, m_Board.Width);
                if (!m_Board.Board[randomRow, randomColumn].Visible)
                {
                    computerMove.Append(convertColumnIntToChar(randomColumn)).Append(convertRowIntToChar(randomRow));
                    break;
                }
            }

            return computerMove.ToString();
        }

        public Player GetWinner()
        {
            int winnerScore = Math.Max(r_Player1.Score, r_Player2.Score);

            return Player1.Score == winnerScore ? Player1 : Player2;
        }

        public bool IsGameOver()
        {
            bool isGameOver;

            if (areAllCellsVisible())
            {
                isGameOver = true;
                m_GameStatus = Player1.Score == Player2.Score ? eGameStatus.Tie : eGameStatus.Win;
            }
            else
            {
                isGameOver = false;
                m_GameStatus = eGameStatus.InProcess;
            }

            return isGameOver;
        }
    }
}
