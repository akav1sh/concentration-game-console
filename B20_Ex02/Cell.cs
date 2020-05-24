namespace B20_Ex02
{
    public class Cell<T>
    {
        private readonly T r_Content;
        private readonly int r_Row;
        private readonly int r_Column;
        private bool m_IsVisible;

        public Cell(T i_Content, int i_Row, int i_Column)
        {
            r_Content = i_Content;
            m_IsVisible = false;
            r_Row = i_Row;
            r_Column = i_Column;
        }

        public T Content
        {
            get
            {
                return r_Content;
            }
        }

        public bool IsVisible
        {
            get
            {
                return m_IsVisible;
            }

            set
            {
                m_IsVisible = value;
            }
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
