namespace B20_Ex02
{
    public class GameLogic
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly eGameMode r_Mode;
        private Player m_CurrentPlayer;
        private GameBoard m_Board;
        
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
        }

        public void SetNewBoard(int i_Height, int i_Width)
        {
            m_Board = new GameBoard(i_Height, i_Width);
        }

        public bool IsValidCellInput(string i_CellToCheck)
        {
            bool isValidCell;

            if (i_CellToCheck.Length != 2 || !char.IsUpper(i_CellToCheck[0]) || !char.IsDigit(i_CellToCheck[1]))
            {
                isValidCell = false;
            }
            else
            {
                int column = ExtractColumn(i_CellToCheck[0]);
                int row = ExtractRow(i_CellToCheck[1]);
                isValidCell = column >= 0 && column <= m_Board.Width - 1 && row >= 0 && row <= m_Board.Height - 1;
            }

            return isValidCell;
        }

        public bool IsValidName(string i_NameToCheck)
        {
            return string.IsNullOrEmpty(i_NameToCheck);
        }

        public bool isValidGameModeSelection(string i_GameMode)
        {
            return int.TryParse(i_GameMode, out int mode) && ((eGameMode)mode == eGameMode.PlayerVsComputer || (eGameMode)mode == eGameMode.PlayerVsComputer);
        }

        public bool IsValidRowOrColumnSize(string i_Size)
        {
           return int.TryParse(i_Size, out int size)
                   && size >= m_Board.MinRowOrColumnSize && size <= m_Board.MaxRowOrColumnSize;
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

        public string ChooseComputerMove() // TODO
        {
            return null;
        }

        public bool IsGameOver() // TODO
        {
            return false;
        }
    }
}
