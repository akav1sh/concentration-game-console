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
        public static readonly Random sr_ComputerRandomizer = new Random();
        
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
            InvalidCellBorders,
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
            return !string.IsNullOrEmpty(i_NameToCheck);
        }

        public static bool IsValidGameModeSelection(string i_GameMode)
        {
            return int.TryParse(i_GameMode, out int mode) && ((eGameMode)mode == eGameMode.PlayerVsPlayer || (eGameMode)mode == eGameMode.PlayerVsComputer);
        }

        public void SetNewBoard(int i_Height, int i_Width)
        {
            m_Board = new GameBoard(i_Height, i_Width);
        }

        public bool IsValidCellInput(string i_CellToCheck, out eInputCellStatus o_Status)
        {
            bool isValidCell;

            if (i_CellToCheck == "Q")
            {
                isValidCell = false;
                o_Status = eInputCellStatus.QuitGame;
            }
            else
            {
                if (i_CellToCheck.Length != 2 || !char.IsUpper(i_CellToCheck[0]) || !char.IsDigit(i_CellToCheck[1]))
                {
                    isValidCell = false;
                    o_Status = eInputCellStatus.InvalidCellFormat;
                }
                else
                {
                    int column = ExtractColumn(i_CellToCheck[0]);
                    int row = ExtractRow(i_CellToCheck[1]);
                    if (!(column >= 0 && column <= m_Board.Width - 1 && row >= 0 &&
                          row <= m_Board.Height - 1))
                    {
                        isValidCell = false;
                        o_Status = eInputCellStatus.InvalidCellBorders;
                    }
                    else
                    {
                        if (IsCellVisible(row, column))
                        {
                            isValidCell = false;
                            o_Status = eInputCellStatus.VisibleCell;
                        }
                        else
                        {
                            isValidCell = true;
                            o_Status = eInputCellStatus.ValidCell;
                        }
                    }
                }
            }

            return isValidCell;
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
                if (height < GameBoard.sr_MinHeightOrWidth || height > GameBoard.sr_MaxHeightOrWidth ||
                    width < GameBoard.sr_MinHeightOrWidth || width > GameBoard.sr_MaxHeightOrWidth)
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

        public int ExtractColumn(char i_Column)
        {
            return i_Column - 'A';
        }

        public int ExtractRow(char i_Row)
        {
            return int.Parse(i_Row.ToString()) - 1;
        }

        public void SetPlayerForNextMove(Player i_Player)
        {
            m_CurrentPlayer = i_Player;
        }

        public void ChooseComputerCell(out int o_ChosenRow, out int o_ChosenColumn)
        {
            m_CurrentPlayer = r_Player2;
            int randomRow, randomColumn;
            const bool v_Visible = true;

            while (true)
            {
                randomRow = sr_ComputerRandomizer.Next(0, m_Board.Height);
                randomColumn = sr_ComputerRandomizer.Next(0, m_Board.Width);
                if (IsCellVisible(randomRow, randomColumn) == false)
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

        public bool IsGameOver(out eGameOverStatus o_Status)
        {
            bool isGameOver;

            if (m_Board.AreAllCellsVisible())
            {
                isGameOver = true;
                o_Status = Player1.Score == Player2.Score ? eGameOverStatus.Tie : eGameOverStatus.Win;
            }
            else
            {
                isGameOver = false;
                o_Status = eGameOverStatus.GameNotOver;
            }

            return isGameOver;
        }
    }
}
