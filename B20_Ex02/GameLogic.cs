using System;

namespace B20_Ex02
{
    public class GameLogic
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly eGameMode r_Mode;
        private Player m_CurrentPlayer;
        private GameBoard m_Board;
        internal static readonly Random sr_RandGenerator = new Random();
        
        public GameLogic(Player i_Player1, Player i_Player2, eGameMode i_GameMode)
        {
            r_Player1 = i_Player1;
            r_Player2 = i_Player2;
            m_CurrentPlayer = null;
            r_Mode = i_GameMode;
        }

        public enum eGameMode
        {
            PlayerVsPlayer = 1,
            PlayerVsComputer = 2
        }

        public enum eInputCellStatus
        {
            InvalidCellFormat,
            InvalidCellBounds,
            VisibleCell,
            QuitGame,
            ValidCell
        }

        public enum eGameOverStatus
        {
            Win,
            Tie,
            GameNotOver
        }

        public GameBoard Board
        {
            get
            {
                return m_Board;
            }
        }

        public eGameMode Mode
        {
            get
            {
                return r_Mode;
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
        }

        public bool IsValidInputCell(string i_CellToCheck, out eInputCellStatus o_Status)
        {
            bool validCell;

            if (i_CellToCheck == "Q")
            {
                validCell = false;
                o_Status = eInputCellStatus.QuitGame;
            }
            else
            {
                if (i_CellToCheck.Length != 2 || !char.IsUpper(i_CellToCheck[0]) || 
                    !char.IsDigit(i_CellToCheck[1]))
                {
                    validCell = false;
                    o_Status = eInputCellStatus.InvalidCellFormat;
                }
                else
                {
                    int column = ExtractColumn(i_CellToCheck[0]);
                    int row = ExtractRow(i_CellToCheck[1]);
                    if (column < 0 || column > m_Board.Width - 1 || row < 0 || row > m_Board.Height - 1)
                    {
                        validCell = false;
                        o_Status = eInputCellStatus.InvalidCellBounds;
                    }
                    else
                    {
                        if (IsCellVisible(row, column))
                        {
                            validCell = false;
                            o_Status = eInputCellStatus.VisibleCell;
                        }
                        else
                        {
                            validCell = true;
                            o_Status = eInputCellStatus.ValidCell;
                        }
                    }
                }
            }

            return validCell;
        }

        public bool IsValidBoardSize(string i_Height, string i_Width)
        {
            bool validBoardSize;
            bool validHeight = int.TryParse(i_Height, out int height);
            bool validWidth = int.TryParse(i_Width, out int width);

            if (!validHeight || !validWidth)
            {
                validBoardSize = false;
            }
            else
            {
                if (height < GameBoard.k_MinHeightOrWidth || height > GameBoard.k_MaxHeightOrWidth ||
                    width < GameBoard.k_MinHeightOrWidth || width > GameBoard.k_MaxHeightOrWidth)
                {
                    validBoardSize = false;
                }
                else
                {
                    validBoardSize = (height * width) % 2 == 0;
                }
            }

            return validBoardSize;
        }

        public int ExtractColumn(char i_Column)
        {
            return i_Column - 'A';
        }

        public int ExtractRow(char i_Row)
        {
            return int.Parse(i_Row.ToString()) - 1;
        }

        public void SetCurrentPlayerToPlay(Player i_Player)
        {
            m_CurrentPlayer = i_Player;
        }

        public void ChooseComputerCell(out int o_ChosenRow, out int o_ChosenColumn)
        {
            int randomRow, randomColumn;
            const bool v_Visible = true;

            while (true)
            {
                randomRow = sr_RandGenerator.Next(0, m_Board.Height);
                randomColumn = sr_RandGenerator.Next(0, m_Board.Width);
                if (!IsCellVisible(randomRow, randomColumn))
                {
                    o_ChosenRow = randomColumn;
                    o_ChosenColumn = randomColumn;
                    m_Board.SetCellState(randomRow, randomColumn, v_Visible);
                    break;
                }
            }
        }

        public bool IsCellVisible(int i_Row, int i_Column)
        {
            return m_Board.Board[i_Row, i_Column].Visible;
        }

        private bool areAllCellsVisible()
        {
            return m_Board.HiddenCellsAmount == 0;
        }

        public bool IsGameOver(out eGameOverStatus o_Status)
        {
            bool gameOver;

            if (areAllCellsVisible())
            {
                gameOver = true;
                o_Status = Player1.Score == Player2.Score ? eGameOverStatus.Tie : eGameOverStatus.Win;
            }
            else
            {
                gameOver = false;
                o_Status = eGameOverStatus.GameNotOver;
            }

            return gameOver;
        }
    }
}
