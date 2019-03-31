using System;
using System.Collections.Generic;
using System.Text;

namespace Project2
{
    public struct Move
    {
        private SquarePosition m_Start;
        private SquarePosition m_End;
        
        public Move(SquarePosition i_Start, SquarePosition i_End)
        {
            m_Start = i_Start;
            m_End = i_End;
        }

        public SquarePosition Start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public SquarePosition End
        {
            get { return m_End; }
            set { m_End = value; }
        }

        public bool QuitRoundOrConvertStringToMove(string i_Move)
        {
            bool isQuit = false;

            if (i_Move != null)
            {
                m_Start.X = i_Move[0] - 'A';
                m_Start.Y = i_Move[1] - 'a';
                m_End.X = i_Move[3] - 'A';
                m_End.Y = i_Move[4] - 'a';
            }
            else
            {
                isQuit = true;
            }

            return isQuit;
        }

        public bool CheckIfEatingMove()
        {
            bool isEatingMove = false;

            if (m_End.X - m_Start.X == 2 || m_End.X - m_Start.X == -2)
            {
                isEatingMove = true;
            }

            return isEatingMove;
        }
    }
}
