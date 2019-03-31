using System;
using System.Collections.Generic;
using System.Text;

namespace Project2
{
    public enum ePlayerColor
    {
        White = 1,
        Black = -1
    }

    class Game
    {
        const int k_SinglePlayer = 1;
        const int k_MultiPlayer = 2;

        private Checker[,] m_Board;
        private ePlayerColor m_CurrentTurn;
        private int m_WhitesCounter;
        private int m_BlacksCounter;
        Player m_Player1;
        Player m_Player2;
        Move m_CurrentMove;

        

        public Game(int i_BoardSize, Player i_Player1, Player i_Player2)
        {
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            initializeBoard(i_BoardSize);
        }

        public ePlayerColor CurrentTurn
        {
            get { return m_CurrentTurn; }    
        }

        public Move CurrentMove
        {
            get { return m_CurrentMove; }
            set { m_CurrentMove = value; }
        }

        public Checker[,] Board
        {
            get { return m_Board; }
        }

        public int WhitesCounter
        {
            get { return m_WhitesCounter; }
            set { m_WhitesCounter = value; }
        }

        public int BlacksCounter
        {
            get { return m_BlacksCounter; }
            set { m_BlacksCounter = value; }
        }


        public void initializeBoard(int i_BoardSize)
        {
            m_CurrentMove = new Move();
            m_CurrentTurn = ePlayerColor.White;
            m_Board = new Checker[i_BoardSize, i_BoardSize];
            m_BlacksCounter = 0;
            m_WhitesCounter = 0;
            ColorToss();

            for (int i = 0; i < (i_BoardSize / 2) - 1; i++)           // initialize the black checkers on the board
            {
                for (int j = (i + 1) % 2; j < i_BoardSize; j += 2)
                {
                    m_Board[i, j] = new Checker(ePlayerColor.Black, new SquarePosition(i, j));
                    m_BlacksCounter++;
                }
            }

            for (int i = i_BoardSize - 1; i > i_BoardSize / 2; i--)       // initialize the white checkers on the board
            {
                for (int j = (i + 1) % 2; j < i_BoardSize; j += 2)
                {
                    m_Board[i, j] = new Checker(ePlayerColor.White, new SquarePosition(i, j));
                    m_WhitesCounter++;
                }
            }
        }

        public void ColorToss()
        {
            Random number = new Random();

            if (number.Next() % 2 == 0)             //deciding who is going first as whites.
            {
                m_Player1.Color = ePlayerColor.White;
                m_Player2.Color = ePlayerColor.Black;
            }
            else
            {
                m_Player1.Color = ePlayerColor.Black;
                m_Player2.Color = ePlayerColor.White;
            }
        }

        public bool MakeMove(string i_MoveString = null)
        {
            bool isLegalMove = true;               // returns null if current Player pressed Q or Has no possible moves 
            updateCurrentPossibleMovesInCheckers();
            List<Move> possibleMoves = TotalPossibleCurrentMoves();

            if (i_MoveString == null && possibleMoves.Count > 0)
            {
                Random index = new Random();
                m_CurrentMove = possibleMoves[index.Next(0, possibleMoves.Count - 1)];        // using Random.Next() method as required to choose from possible moves
            }
            else if (possibleMoves.Count > 0)
            {
                if (m_CurrentMove.QuitRoundOrConvertStringToMove(i_MoveString))
                {
                    endRound();     // if returns true, means current player Quit the round with 'Q' , he is the loser in the round
                }
                             
                isLegalMove = checkIfCurrentMoveIsPossible(possibleMoves);                
            }
            else
            {
                endRound();     // no possible moves to current player, he loses
            }

            return isLegalMove;
        }

        public List<Move> TotalPossibleCurrentMoves()
        {
            List<Move> totalPossibleMoves = new List<Move>();

            foreach (Checker checker in m_Board)
            {
                if (checker != null && checker.Color == m_CurrentTurn && checker.PossibleMoves != null)
                {
                    foreach (Move move in checker.PossibleMoves)                                    // checking for eating moves first
                    {
                        if (move.CheckIfEatingMove())
                        {
                            totalPossibleMoves.Add(move);
                        }
                    }
                }
            }

            if (totalPossibleMoves.Count == 0)               // if there is no eating moves then add all regular moves
            {
                foreach (Checker checker in m_Board)
                {
                    if (checker != null && checker.Color == m_CurrentTurn && checker.PossibleMoves != null)
                    {
                        totalPossibleMoves.AddRange(checker.PossibleMoves);
                    }
                }
            }

            return totalPossibleMoves;
        }

        public List<Move> CheckAdditionalEatingMove()
        {
            Checker eatingChecker = m_Board[m_CurrentMove.End.Y, m_CurrentMove.End.X];

            if (eatingChecker.Type == Checker.eCheckerType.Soldier)
            {
                eatingChecker.insertSoldierEatingMovesToList(m_Board);
            }
            else if (eatingChecker.Type == Checker.eCheckerType.King)
            {
                eatingChecker.insertKingEatingMovesToList(m_Board);
            }

            return eatingChecker.PossibleMoves;
        }

        public bool thereIsEatingMoves(List<Move> i_PossibleMoves)
        {
            bool thereIs = false;

            if (i_PossibleMoves.Count > 0)
            {
                thereIs = true;
            }

            return thereIs;

        }

        public bool MakeAdditionalEatingMove(string i_CurrentMove = null)
        {
            bool isLegal = false;

            if (m_CurrentMove.QuitRoundOrConvertStringToMove(i_CurrentMove))
            {
                if (checkIfCurrentMoveIsPossible(CheckAdditionalEatingMove()))
                {
                    isLegal = true;
                }
            }
            else
            {
                endRound();
            }

            return isLegal;
        }

        public void updateBoard()
        {
            m_Board[m_CurrentMove.End.Y, m_CurrentMove.End.X] = new Checker(m_CurrentTurn, m_CurrentMove.End, m_Board[m_CurrentMove.Start.Y, m_CurrentMove.Start.X].Type);
            m_Board[m_CurrentMove.Start.Y, m_CurrentMove.Start.X] = null;

            if (m_CurrentMove.End.Y == 0 || m_CurrentMove.End.Y == (m_Board.GetLength(0) - 1))
            {
                m_Board[m_CurrentMove.End.Y, m_CurrentMove.End.X].Type = Checker.eCheckerType.King;
            }

            if (m_CurrentMove.CheckIfEatingMove())
            {
                int i = (m_CurrentMove.End.Y + m_CurrentMove.Start.Y) / 2;
                int j = (m_CurrentMove.End.X + m_CurrentMove.Start.X) / 2;

                m_Board[i, j] = null;       // eaten checker becomes empty square on the board

                if (m_CurrentTurn == ePlayerColor.White)
                {
                    m_WhitesCounter--;
                }
                else
                {
                    m_BlacksCounter--;
                }
            }
        }

        public void updateCurrentTurn()
        {
            m_CurrentTurn = (ePlayerColor)((int)m_CurrentTurn * -1);
        }

        public void updateCurrentPossibleMovesInCheckers()
        {
            foreach (Checker checker in m_Board)
            {
                if (checker != null && checker.Color == m_CurrentTurn)
                {
                    checker.InsertPossibleCorrectMovesToList(m_Board);
                }
            }
        }

        public bool checkIfCurrentMoveIsPossible(List<Move> i_PossibleMoves)
        {
            bool isPossible = false;

            foreach (Move move in i_PossibleMoves)
            {
                if (move.Equals(m_CurrentMove))
                {
                    isPossible = true;
                    break;
                }
            }

            return isPossible;
        }

        public Player CurrentPlayer()
        {
            if (m_CurrentTurn == m_Player1.Color)
            {
                return m_Player1;
            }
            else
            {
                return m_Player2;
            }
        }

        public int calculateScore()
        {
            int score = 0;

            foreach (Checker checker in m_Board)
            {
                if (checker != null && checker.Color != m_CurrentTurn)
                {
                    if (checker.Type == Checker.eCheckerType.Soldier)
                    {
                        score++;
                    }
                    else
                    {
                        score += 4;
                    }
                }
                else if (checker != null)
                {
                    if (checker.Type == Checker.eCheckerType.Soldier)
                    {
                        score--;
                    }
                    else
                    {
                        score -= 4;
                    }
                }
            }

            if (score < 0)
            {
                score = 0;
            }

            return score;
        }

        public void endRound()
        {
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    m_Board[i, j] = null;
                }
            }
        }
    }
}
