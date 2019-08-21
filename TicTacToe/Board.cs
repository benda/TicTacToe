using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Board
    {
        public Cell[,] Cells { get; private set; }
        public List<Cell> CellsList { get; private set; }

        public Board()
        {
            Cells = new Cell[3, 3];
            CellsList = new List<Cell>();

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    Cell cell = new Cell(row, column);
                    Cells[row, column] = cell;
                    CellsList.Add(cell);
                }
            }
        }

        public Board Clone()
        {
            Board b = new Board();
            for (int i = 0; i < b.CellsList.Count; i++)
            {
                b.CellsList[i].Content = CellsList[i].Content;
            }
            return b;
        }

        internal GameState GetCurrentGameState()
        {
            for (int row = 0; row < 3; row++)
            {
                Cell cell = Cells[row, 0];
                for (int column = 0; column < 3; column++)
                {
                    if (cell.IsOpen || cell.Content != Cells[row, column].Content)
                    {
                        cell = null;
                        break;
                    }
                }

                if (cell != null)
                {
                    return cell.Content == Cell.HumanMark ? GameState.HumanWin : GameState.AIWin;
                }
            }

            for (int column = 0; column < 3; column++)
            {
                Cell cell = Cells[0, column];
                for (int row = 0; row < 3; row++)
                {
                    if (cell.IsOpen || cell.Content != Cells[row, column].Content)
                    {
                        cell = null;
                        break;
                    }
                }

                if (cell != null)
                {
                    return cell.Content == Cell.HumanMark ? GameState.HumanWin : GameState.AIWin;
                }
            }

            if (CellsList[0].Content == CellsList[4].Content && CellsList[0].Content == CellsList[8].Content && !CellsList[0].IsOpen)
            {
                return CellsList[0].Content == Cell.HumanMark ? GameState.HumanWin : GameState.AIWin;
            }

            if (CellsList[2].Content == CellsList[4].Content && CellsList[2].Content == CellsList[6].Content && !CellsList[2].IsOpen)
            {
                return CellsList[2].Content == Cell.HumanMark ? GameState.HumanWin : GameState.AIWin;
            }

            foreach (Cell cell in Cells)
            {
                if (cell.IsOpen)
                {
                    return GameState.InProgress;
                }
            }

            return GameState.Draw;
        }
    }
}
