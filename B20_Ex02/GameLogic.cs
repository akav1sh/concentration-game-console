using System;

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
            m_CurrentPlayer = i_Player1;
            r_GameMode = i_GameGameMode;
            m_HiddenPairCellsAmount = 0;
            m_GameStatus = eGameStatus.InProcess;
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

            set
            {
                m_HiddenPairCellsAmount = value;
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

        public void SetBoard(int i_Height, int i_Width)
        {
            m_Board = new GameBoard(i_Height, i_Width);
            m_HiddenPairCellsAmount = (i_Height * i_Width) / 2;
        }

        public bool IsValidMove(string i_CellToCheck, out ePlayerMoveStatus o_Status)
        {
            bool isValidMove;

            if (i_CellToCheck == "Q" || i_CellToCheck == "q")
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
                    int column = extractColumn(i_CellToCheck[0]);
                    int row = extractRow(i_CellToCheck[1]);
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
            bool isValidHeight = int.TryParse(i_Height, out int height);
            bool isValidWidth = int.TryParse(i_Width, out int width);

            if (!isValidHeight || !isValidWidth)
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

        private int extractColumn(char i_Column)
        {
            return i_Column - 'A';
        }

        private int extractRow(char i_Row)
        {
            return int.Parse(i_Row.ToString()) - 1;
        }

        public void ToggleCellState(string i_CellToToggle)
        {
            int column = extractColumn(i_CellToToggle[0]);
            int row = extractRow(i_CellToToggle[1]);
            bool updatedCellState = !m_Board.Board[row, column].Visible;

            m_Board.SetCellState(row, column, updatedCellState);
        }

        public bool IsMatch(string i_FirstCell, string i_SecondCell)
        {
            return m_Board[extractRow(i_FirstCell[1]), extractColumn(i_FirstCell[0])] == 
                   m_Board[extractRow(i_SecondCell[1]), extractColumn(i_SecondCell[0])];
        }

        public void TogglePlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public void ResetGame()
        {
            Player1.Score = 0;
            Player2.Score = 0;
        }

        public void ChooseComputerCell(out int o_ChosenRow, out int o_ChosenColumn)
        {
            int randomRow, randomColumn;
            const bool v_Visible = true;

            while (true)
            {
                randomRow = sr_RandGenerator.Next(0, m_Board.Height);
                randomColumn = sr_RandGenerator.Next(0, m_Board.Width);
                if (!m_Board.Board[randomRow, randomColumn].Visible)
                {
                    o_ChosenRow = randomColumn;
                    o_ChosenColumn = randomColumn;
                    m_Board.SetCellState(randomRow, randomColumn, v_Visible);
                    break;
                }
            }
        }

        public Player GetWinner()
        {
            int winnerScore = Math.Max(r_Player1.Score, r_Player2.Score);

            return Player1.Score == winnerScore ? Player1 : Player2;
        }

        private bool areAllCellsVisible()
        {
            return m_HiddenPairCellsAmount == 0;
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
