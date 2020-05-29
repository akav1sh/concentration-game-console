namespace B20_Ex02
{
    internal class Cell<T>
    {
        private readonly T r_Content;
        private bool m_IsVisible;

        internal Cell(T i_Content)
        {
            r_Content = i_Content;
            m_IsVisible = false;
        }

        internal T Content
        {
            get
            {
                return r_Content;
            }
        }

        internal bool Visible
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
