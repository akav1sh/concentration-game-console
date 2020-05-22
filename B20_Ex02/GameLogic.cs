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

        public bool IsValidCell(string i_CellToCheck) // TODO
        {
            bool isValidCell = true;
            int column = extractColumn(i_CellToCheck[0]);
            int row = extractRow(i_CellToCheck[1]);

            return isValidCell;
        }

        public bool IsValidBoardSize(string i_UserInputSize)
        {
           return int.TryParse(i_UserInputSize, out int boardSize)
                   && boardSize >= m_Board.MinSize && boardSize <= m_Board.MaxSize;
        }

        public void ExtractCell(string i_Cell, out int o_Column, out int o_Row)
        {
            o_Column = extractColumn(i_Cell[0]);
            o_Row = extractRow(i_Cell[1]);
        }

        private int extractColumn(char i_Column)
        {
            return i_Column - 'A';
        }

        private int extractRow(char i_Row)
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

        public bool IsCellVisible(int i_Row, int i_Column)
        {
            return m_Board.Board[i_Row, i_Column].Visible;
        }
    }
}
