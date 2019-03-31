using System;
using System.Collections.Generic;
using System.Text;

namespace Project2
{
    public class Checker
    {
        public enum eCheckerType
        {
            Soldier,
            King
        }

        private ePlayerColor m_Color;
        private eCheckerType m_Type;
        private SquarePosition m_Position;


        private List<Move> m_PossibleMoves;

        public Checker(ePlayerColor i_Color, SquarePosition i_Position, eCheckerType i_Type = eCheckerType.Soldier)
        {
            m_Type = i_Type;
            m_Color = i_Color;
            m_Position = i_Position;
            m_PossibleMoves = new List<Move>(4);        // maximum possible moves for one checker (when king) 
        }

        public SquarePosition Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public List<Move> PossibleMoves
        {
            get { return m_PossibleMoves; }
            set { m_PossibleMoves = value; }
        }

        public ePlayerColor Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public eCheckerType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public void InsertPossibleCorrectMovesToList(Checker[,] i_Board)
        {
            bool thereAreEatingMoves = false;

            if (m_Type == eCheckerType.Soldier)
            {
                thereAreEatingMoves = insertSoldierEatingMovesToList(i_Board);
                if (!thereAreEatingMoves)
                {
                    insertSoldierRegularMovesToList(i_Board);
                }
            }
            else if (m_Type == eCheckerType.King)
            {
                thereAreEatingMoves = insertKingEatingMovesToList(i_Board);
                if (!thereAreEatingMoves)
                {
                    insertKingRegularMovesToList(i_Board);
                }
            }
        }

        public ePlayerColor OppositeColor()
        {
            ePlayerColor color = (ePlayerColor)((int)m_Color * -1);

            return color;
        }

        private void checkRightRegularMove(int i_Color, Checker[,] i_Board)
        {
            int i = m_Position.Y;
            int j = m_Position.X;
            int boardSize = i_Board.GetLength(0);

            if (i - i_Color < boardSize &&
                i - i_Color >= 0 &&
                j + 1 < boardSize && 
                i_Board[i - i_Color, j + 1] == null)
            {
                m_PossibleMoves.Add(new Move(m_Position, new SquarePosition(i - i_Color, j + 1)));  //insert possible move to array
            }
        }

        private void checkLeftRegularMove(int i_Color, Checker[,] i_Board)
        {          
            int i = m_Position.Y;
            int j = m_Position.X;
            int boardSize = i_Board.GetLength(0);

            if (i - i_Color < boardSize &&
                i - i_Color >= 0 &&
                j - 1 >= 0 && 
                i_Board[i - i_Color, j - 1] == null)
            {
                m_PossibleMoves.Add(new Move(m_Position, new SquarePosition(i - i_Color, j - 1)));  //insert possible move to array
            }
        }

        private bool checkRightEatingMove(int i_Color, Checker[,] i_Board)
        {
            bool thereIsEatingMove = false;
            int i = m_Position.Y;
            int j = m_Position.X;
            int boardSize = i_Board.GetLength(0);

            if (i - (2 * i_Color) < boardSize &&
                i - (2 * i_Color) >= 0 &&
                j + 2 < boardSize &&
                i_Board[i - i_Color, j + 1] != null &&
                i_Board[i - i_Color, j + 1].Color == OppositeColor())
            {
                if (i_Board[i - (2 * i_Color), j + 2] == null)              // empty square is null  
                {
                    m_PossibleMoves.Add(new Move(m_Position, new SquarePosition(i - (2 * i_Color), j + 2))); //insert move to array
                    thereIsEatingMove = true;
                }
            }

            return thereIsEatingMove;
        }

        private bool checkLeftEatingMove(int i_Color, Checker[,] i_Board)
        {
            bool thereIsEatingMove = false;
            int i = m_Position.Y;
            int j = m_Position.X;
            int boardSize = i_Board.GetLength(0);

            if (i - (2 * i_Color) < boardSize &&
                j - 2 >= 0 &&
                i - (2 * i_Color) >= 0 &&
                i_Board[i - i_Color, j - 1] != null &&
                i_Board[i - i_Color, j - 1].Color == OppositeColor())
            {
                if (i_Board[i - (2 * i_Color), j - 2] == null)
                {
                    m_PossibleMoves.Add(new Move(m_Position, new SquarePosition(i - (2 * i_Color), j - 2)));  //insert move to array
                    thereIsEatingMove = true;
                }
            }

            return thereIsEatingMove;
        }

        private void insertSoldierRegularMovesToList(Checker[,] i_Board)
        {
            checkRightRegularMove((int)m_Color, i_Board);
            checkLeftRegularMove((int)m_Color, i_Board);
        }

        private void insertKingRegularMovesToList(Checker[,] i_Board)
        {
            checkRightRegularMove((int)m_Color, i_Board);
            checkLeftRegularMove((int)m_Color, i_Board);
            checkRightRegularMove((int)OppositeColor(), i_Board);
            checkLeftRegularMove((int)OppositeColor(), i_Board);
        }

        public bool insertSoldierEatingMovesToList(Checker[,] i_Board)
        {
            bool thereAreEatingMoves = false;

            m_PossibleMoves.Clear();
            thereAreEatingMoves = checkRightEatingMove((int)m_Color, i_Board) || 
                                  checkLeftEatingMove((int)m_Color,i_Board);

            return thereAreEatingMoves;
        }

        public bool insertKingEatingMovesToList(Checker[,] i_Board)
        {
            bool thereAreEatingMoves = false;

            m_PossibleMoves.Clear();
            thereAreEatingMoves = checkRightEatingMove((int)m_Color, i_Board) ||
                                  checkLeftEatingMove((int)m_Color, i_Board) ||
                                  checkRightEatingMove((int)OppositeColor(), i_Board) ||
                                  checkLeftEatingMove((int)OppositeColor(), i_Board);

            return thereAreEatingMoves;
        }
    }
}
