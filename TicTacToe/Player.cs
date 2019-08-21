using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Player
    {
        private System.Threading.AutoResetEvent _wait;
        private Cell _nextMove = null;
        private Board _board;
        private Random _random = new Random();
        public bool Easy { get; set; }
        public bool Medium { get; set; }
        public bool Hard { get; set; }

        public Player(bool isHuman, AutoResetEvent wait, Board board)
        {
            this.IsHuman = isHuman;
            this._wait = wait;
            this._board = board;
        }

        public bool IsHuman { get; private set; }

        internal void NextMove()
        {
            if (IsHuman)
            {
                _wait.WaitOne();
                _nextMove.PlaceHuman();
            }
            else
            {
                Cell nextMove = null; 
                if(Easy)
                {
                    nextMove = GetRandomNextMove();
                }
                else if (Medium)
                {
                    nextMove = _random.Next(0, 2) == 0 ? GetRandomNextMove() : GetOptimalNextMove(_board.Clone(), false);
                }
                else
                {
                     nextMove = GetOptimalNextMove(_board.Clone(), false);
                }
                (from cell in _board.CellsList where cell.Row == nextMove.Row && cell.Column == nextMove.Column select cell).Single().PlaceAI();
            }
        }

        private Cell GetOptimalNextMove(Board board, bool human)
        {
            Cell bestMove = new Cell(0, 0);
            Cell nextWinningMove = new Cell(0, 0);

            GameState state = board.GetCurrentGameState();
            if (state == GameState.Draw)
            {
                bestMove.Value = 0;
            }
            else if ((state == GameState.HumanWin && human) || state == GameState.AIWin && !human)
            {
                nextWinningMove.Value = human ? (int)GameState.HumanWin : (int)GameState.AIWin;
                return nextWinningMove;
            }
            else
            {
                bestMove.Value = human ? (int)GameState.AIWin : (int)GameState.HumanWin;
                foreach (Cell cell in board.Cells)
                {
                    if (cell.IsOpen)
                    {
                        if (human)
                        {
                            cell.PlaceHuman();
                        }
                        else
                        {
                            cell.PlaceAI();
                        }
                        Cell nextMove = GetOptimalNextMove(board, !human);
                        if ((human && nextMove.Value < bestMove.Value) || (!human && nextMove.Value > bestMove.Value))
                        {
                            bestMove = new Cell(cell.Row, cell.Column);
                            bestMove.Value = nextMove.Value;
                        }
                        cell.MarkOpen();                       
                    }
                }
            }
            return bestMove;
        }

        private Cell GetRandomNextMove()
        {
            while (true)
            {
                Cell cell = _board.Cells[_random.Next(0, 3), _random.Next(0, 3)];
                if (cell.IsOpen)
                {
                    return cell;
                }
            }
        }

        internal void SetNextMove(Cell p)
        {
            _nextMove = p;
            _wait.Set();
        }
    }
}
