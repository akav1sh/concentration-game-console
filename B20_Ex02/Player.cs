using System;

namespace B20_Ex02
{
    public class Player
    {
        private readonly string r_Name;
        private int m_Score;

        public Player(string i_PlayerName)
        {
            r_Name = i_PlayerName;
            m_Score = 0;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }
    }
}
