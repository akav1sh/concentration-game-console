namespace B20_Ex02
{
    public class GameBoard
    {
        private const int k_MinRowOrColumnSize = 4;
        private const int k_MaxRowOrColumnSize = 6;
        private readonly int r_Height;
        private readonly int r_Width;
        private readonly Cell<char>[,] r_Board;
        private int m_HiddenCellsAmount;

        public GameBoard(int i_Height, int i_Width)
        {
            r_Height = i_Height;
            r_Width = i_Width;
            r_Board = new Cell<char>[r_Height, r_Width];
            m_HiddenCellsAmount = r_Height * r_Width;
            initializeBoard();
        }

        public Cell<char>[,] Board
        {
            get
            {
                return r_Board;
            }
        }

        public int MinRowOrColumnSize
        {
            get
            {
                return k_MinRowOrColumnSize;
            }
        }

        public int MaxRowOrColumnSize
        {
            get
            {
                return k_MaxRowOrColumnSize;
            }
        }

        public int Height
        {
            get
            {
                return r_Height;
            }
        }

        public int Width
        {
            get
            {
                return r_Width;
            }
        }

        public int HiddenCellsAmount
        {
            get
            {
                return m_HiddenCellsAmount;
            }
        }

        public void SetCellState(int i_Row, int i_Column, bool i_Visible)
        {
            r_Board[i_Row, i_Column].Visible = i_Visible;
        }

        private void initializeBoard() // TODO
        {
        }

        public bool AreAllCellsVisible()
        {
            return m_HiddenCellsAmount == 0;
        }
    }
}
