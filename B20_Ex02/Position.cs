namespace B20_Ex02
{
    public struct Position
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

        public static bool operator !=(Position i_FirstPosition, Position i_SecondPosition)
        {
            return !(i_FirstPosition == i_SecondPosition);
        }

        public static bool operator ==(Position i_FirstPosition, Position i_SecondPosition)
        {
            return i_FirstPosition.Equals(i_SecondPosition);
        }

        public override bool Equals(object i_ObjectToCompare)
        {
            bool isEquals = false;

            if (i_ObjectToCompare != null)
            {
                isEquals = i_ObjectToCompare is Position positionToCompare && r_Row == positionToCompare.Row && r_Column == positionToCompare.Column;
            }

            return isEquals;
        }

        public override int GetHashCode()
        {
            return r_Row ^ r_Column;
        }
    }
}
