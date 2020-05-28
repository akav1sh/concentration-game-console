using System;
using System.Text;

namespace B20_Ex02
{
    public class GameLogic
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly eGameMode r_GameMode;
        private AI m_AI;
        private eGameStatus m_GameStatus;
        private Player m_CurrentPlayer;
        private GameBoard m_Board;
        private int m_HiddenPairCellsAmount;
        internal static readonly Random sr_RandGenerator = new Random();
        
        public GameLogic(Player i_Player1, Player i_Player2, eGameMode i_GameMode)
        {
            r_Player1 = i_Player1;
            r_Player2 = i_Player2;
            r_GameMode = i_GameMode;
            m_AI = null;
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
        }

        public static bool IsValidPlayerName(string i_NameToCheck)
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

            if (r_GameMode == eGameMode.PlayerVsComputer)
            {
                m_AI = new AI(m_Board.CreateCellPositionsList());
            }
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

        public bool CheckForMatch(string i_FirstMoveStr, string i_SecondMoveStr)
        {
            Position firstPosition = new Position(convertRowCharToInt(i_FirstMoveStr[1]), convertColumnCharToInt(i_FirstMoveStr[0]));
            Position secondPosition = new Position(convertRowCharToInt(i_SecondMoveStr[1]), convertColumnCharToInt(i_SecondMoveStr[0]));
            bool areContentsMatch = this.arePositionContentsMatch(firstPosition, secondPosition);

            if (areContentsMatch)
            {
                m_HiddenPairCellsAmount--;
                m_CurrentPlayer.Score++;
            }
            else
            {
                ToggleCellState(i_FirstMoveStr);
                ToggleCellState(i_SecondMoveStr);
                togglePlayer();
            }

            if (r_GameMode == eGameMode.PlayerVsComputer)
            {
                m_AI.UpdateDataAccordingToPairing(areContentsMatch, firstPosition, secondPosition);
            }

            return areContentsMatch;
        }

        private bool arePositionContentsMatch(Position i_FirstPosition, Position i_SecondPosition)
        { 
            return m_Board[i_FirstPosition.Row, i_FirstPosition.Column] == m_Board[i_SecondPosition.Row, i_SecondPosition.Column];
        }

        private void togglePlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public void ResetGame()
        {
            m_CurrentPlayer = Player1;
            Player1.Score = 0;
            Player2.Score = 0;
            m_GameStatus = eGameStatus.InProcess;
        }

        public string ChooseComputerMove()
        {
            StringBuilder computerMove = new StringBuilder();
            Position randPosition;

            if (m_AI.IsFirstMove)
            {
               randPosition = m_AI.ChooseRandomHiddenCellPosition();
               m_AI.RemovePositionFromHiddenCellPositions(randPosition);
               m_AI.FirstMove = randPosition;
            }
            else
            {
                Position? pairPosition = findPairPositionInAI();
                if (pairPosition == null)
                {
                    randPosition = m_AI.ChooseRandomHiddenCellPosition();
                }
                else
                {
                    randPosition = (Position)pairPosition;
                }
            }

            m_AI.IsFirstMove = !m_AI.IsFirstMove;
            computerMove.Append(convertColumnIntToChar(randPosition.Column)).Append(convertRowIntToChar(randPosition.Row));

            return computerMove.ToString();
        }

        private Position? findPairPositionInAI()
        {
            Position? pairPosition = null;

            if (m_AI.FirstMove != null)
            {
                foreach (Position knownCellPosition in m_AI.KnownCellPositions)
                {
                    if (knownCellPosition != m_AI.FirstMove && arePositionContentsMatch((Position)m_AI.FirstMove, knownCellPosition))
                    {
                        pairPosition = knownCellPosition;
                        break;
                    }
                }
            }

            return pairPosition;
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
