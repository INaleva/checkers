using System;
using System.Collections.Generic;
using System.Text;

namespace Project2
{
    public struct SquarePosition
    {
        private int m_y;
        private int m_x;

        public SquarePosition(int i_Y, int i_X)
        {
            m_x = i_X;
            m_y = i_Y;
        }
        //copy constructor
        public SquarePosition(SquarePosition i_squarePosition)
        {
            m_x = i_squarePosition.X;
            m_y = i_squarePosition.Y;
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }
    }
}
