namespace B20_Ex02
{
    public class Player
    {
        private readonly string r_Name;
        private readonly ePlayerType r_PlayerType;
        private int m_Score;

        public Player(string i_PlayerName, ePlayerType i_PlayerType)
        {
            r_Name = i_PlayerName;
            r_PlayerType = i_PlayerType;
            m_Score = 0;
        }

        public enum ePlayerType
        {
            Human,
            AI
        }

        public ePlayerType PlayerType
        {
            get
            {
                return r_PlayerType;
            }
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

            internal set
            {
                m_Score = value;
            }
        }
    }
}
