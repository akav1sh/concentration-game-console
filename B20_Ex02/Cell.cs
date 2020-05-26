namespace B20_Ex02
{
    public class Cell<T>
    {
        private readonly T r_Content;
        private readonly Position r_Position;
        private bool m_IsVisible;

        public Cell(T i_Content, Position i_Position)
        {
            r_Content = i_Content;
            r_Position = i_Position;
            m_IsVisible = false;
        }

        public T Content
        {
            get
            {
                return r_Content;
            }
        }

        public bool Visible
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

        public Position Position
        {
            get
            {
                return r_Position;
            }
        }
    }
}
