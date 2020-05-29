using System.Collections.Generic;
using System.Drawing;

namespace B20_Ex02
{
    internal class AI
    {
        private List<Point> m_HiddenCellPositions;
        private List<Point> m_KnownCellPositions;
        private bool m_IsFirstMove;
        private Point? m_FirstMove;

        internal AI(List<Point> i_CellPositions)
        {
            m_HiddenCellPositions = i_CellPositions;
            m_KnownCellPositions = new List<Point>(i_CellPositions.Count);
            m_IsFirstMove = true;
            m_FirstMove = null;
        }

        internal List<Point> KnownCellPositions
        {
            get
            {
                return m_KnownCellPositions;
            }
        }

        internal List<Point> HiddenCellPositions
        {
            get
            {
                return m_HiddenCellPositions;
            }
        }

        internal bool IsFirstMove
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

        internal Point? FirstMove
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

        internal Point ChooseRandomHiddenCellPosition()
        {
            return m_HiddenCellPositions[GameLogic.sr_RandGenerator.Next(0, m_HiddenCellPositions.Count)];
        }

        private void addPositionToKnownCellPositions(Point i_Position)
        {
            if (!m_KnownCellPositions.Contains(i_Position))
            {
                m_KnownCellPositions.Add(i_Position);
            }
        }

        private void addPositionToHiddenCellPositions(Point i_Position)
        {
            if (!m_HiddenCellPositions.Contains(i_Position))
            {
                m_HiddenCellPositions.Add(i_Position);
            }
        }

        internal void RemovePositionFromHiddenCellPositions(Point i_Position)
        {
            m_HiddenCellPositions.Remove(i_Position);
        }

        private void removePositionFromKnownCellPositions(Point i_Position)
        {
            m_KnownCellPositions.Remove(i_Position);
        }

        internal void UpdateDataAccordingToPairing(bool i_AreContentsMatch, Point i_FirstPosition, Point i_SecondPosition)
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
