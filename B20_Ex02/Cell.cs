namespace B20_Ex02
{
    public class Cell<T>
    {
        private readonly T r_Content;
        private bool m_IsVisible;

        public Cell(T i_Content)
        {
            r_Content = i_Content;
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
    }
}
