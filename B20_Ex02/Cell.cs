using System;

namespace B20_Ex02
{
    public class Cell<T>
    {
        private bool m_Visible;
        private T m_Content;

        public Cell()
        {
            m_Visible = false;
            m_Content = default;
        }

        public Cell(T i_Content)
        {
            m_Visible = false;
            m_Content = i_Content;
        }

        public T Content
        {
            get
            {
                return m_Content;
            }

            set
            {
                m_Content = value;
            }
        }

        public bool Visible
        {
            get
            {
                return m_Visible;
            }

            set
            {
                m_Visible = value;
            }
        }
    }
}
