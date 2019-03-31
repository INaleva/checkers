using System;
using System.Collections.Generic;
using System.Text;

namespace Project2
{
    public class Player
    {
        public enum ePlayerType
        {
            Human,
            Computer
        }

        private string m_Name;
        private int m_Score;
        private ePlayerType m_Type;
        private ePlayerColor m_Color;

        public ePlayerColor Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public Player(ePlayerType i_Type = ePlayerType.Computer)
        {
            if (i_Type == ePlayerType.Computer)
            {
                m_Name = "Robot";
            }

            m_Score = 0;
            m_Type = i_Type;
        }

        public ePlayerType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

    }
}
