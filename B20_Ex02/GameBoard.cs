using System.Collections.Generic;

namespace B20_Ex02
{
    public class GameBoard
    {
        public const int k_MinHeightOrWidth = 2; // 4!!!!! 
        public const int k_MaxHeightOrWidth = 4; // 6!!!!!
        private readonly int r_Height;
        private readonly int r_Width;
        private readonly Cell<char>[,] r_Board;

        public GameBoard(int i_Height, int i_Width)
        {
            r_Height = i_Height;
            r_Width = i_Width;
            r_Board = new Cell<char>[r_Height, r_Width];
            initializeBoard();
        }

        public Cell<char>[,] Board
        {
            get
            {
                return r_Board;
            }
        }

        public char this[int i_Row, int i_Column]
        {
            get
            {
                return r_Board[i_Row, i_Column].Content;
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

        public void SetCellState(int i_Row, int i_Column, bool i_State)
        {
            r_Board[i_Row, i_Column].Visible = i_State;
        }

        private void initializeBoard()
        {
            List<char> cellContents = createCellsContentsList();
            List<Cell<char>> cellPositions = createCellPositionsList();
            int randContentIndex;
            char randContentToAdd;

            while (cellPositions.Count > 0)
            {
                randContentIndex = GameLogic.sr_RandGenerator.Next(0, cellContents.Count);
                randContentToAdd = cellContents[randContentIndex];
                addRandomContentToRandomCell(cellPositions, randContentToAdd);
                addRandomContentToRandomCell(cellPositions, randContentToAdd);
                cellContents.RemoveAt(randContentIndex);
            }
        }

        private void addRandomContentToRandomCell(List<Cell<char>> i_CellPositions, char i_ContentToAdd)
        {
            int randPosition = GameLogic.sr_RandGenerator.Next(0, i_CellPositions.Count);
            int row = i_CellPositions[randPosition].Row;
            int column = i_CellPositions[randPosition].Column;

            r_Board[row, column] = new Cell<char>(i_ContentToAdd, row, column);
            i_CellPositions.RemoveAt(randPosition);
        }

        private List<char> createCellsContentsList()
        {
            List<char> cellContents = new List<char>(26);
            char letter = 'A';

            for (int i = 0; i < cellContents.Capacity; i++, letter++)
            {
                cellContents.Add(letter);
            }

            return cellContents;
        }

        private List<Cell<char>> createCellPositionsList()
        {
            List<Cell<char>> cellPositions = new List<Cell<char>>(r_Height * r_Width);

            for (int i = 0; i < r_Height; i++)
            {
                for (int j = 0; j < r_Width; j++)
                {
                    cellPositions.Add(new Cell<char>(default, i, j));
                }
            }

            return cellPositions;
        }
    }
}
