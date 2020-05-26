namespace B20_Ex02
{
    public class Position
    {
        private readonly int r_Row;
        private readonly int r_Column;

        public Position(int i_Row, int i_Column)
        {
            r_Row = i_Row;
            r_Column = i_Column;
        }

        public int Row
        {
            get
            {
                return r_Row;
            }
        }

        public int Column
        {
            get
            {
                return r_Column;
            }
        }
    }
}
