using System.Collections.Generic;

namespace B20_Ex02
{
    internal class AI
    {
        private List<Position> m_HiddenCellPositions;
        private List<Position> m_KnownCellPositions;
        private bool m_IsFirstMove;
        private Position? m_FirstMove;

        public AI(List<Position> i_CellPositions)
        {
            m_HiddenCellPositions = i_CellPositions;
            m_KnownCellPositions = new List<Position>(i_CellPositions.Count);
            m_IsFirstMove = true;
            m_FirstMove = null;
        }

        public List<Position> KnownCellPositions
        {
            get
            {
                return m_KnownCellPositions;
            }
        }

        public List<Position> HiddenCellPositions
        {
            get
            {
                return m_HiddenCellPositions;
            }
        }

        public bool IsFirstMove
        {
            get
            {
                return m_IsFirstMove;
            }

            set
            {
                m_IsFirstMove = value;
            }
        }

        public Position? FirstMove
        {
            get
            {
                return m_FirstMove;
            }

            set
            {
                m_FirstMove = value;
            }
        }

        public Position ChooseRandomHiddenCellPosition()
        {
            return m_HiddenCellPositions[GameLogic.sr_RandGenerator.Next(0, m_HiddenCellPositions.Count)];
        }

        private void addPositionToKnownCellPositions(Position i_Position)
        {
            if (!m_KnownCellPositions.Contains(i_Position))
            {
                m_KnownCellPositions.Add(i_Position);
            }
        }

        private void addPositionToHiddenCellPositions(Position i_Position)
        {
            if (!m_HiddenCellPositions.Contains(i_Position))
            {
                m_HiddenCellPositions.Add(i_Position);
            }
        }

        public void RemovePositionFromHiddenCellPositions(Position i_Position)
        {
            m_HiddenCellPositions.Remove(i_Position);
        }

        private void removePositionFromKnownCellPositions(Position i_Position)
        {
            m_KnownCellPositions.Remove(i_Position);
        }

        public void UpdateDataAccordingToPairing(bool i_AreContentsMatch, Position i_FirstPosition, Position i_SecondPosition)
        {
           RemovePositionFromHiddenCellPositions(i_FirstPosition);
           RemovePositionFromHiddenCellPositions(i_SecondPosition);

           if (i_AreContentsMatch)
           {
               removePositionFromKnownCellPositions(i_FirstPosition);
               removePositionFromKnownCellPositions(i_SecondPosition);
           }
           else
           {
               addPositionToHiddenCellPositions(i_FirstPosition);
               addPositionToHiddenCellPositions(i_SecondPosition);
               addPositionToKnownCellPositions(i_FirstPosition);
               addPositionToKnownCellPositions(i_SecondPosition);
           }
        }
    }
}
