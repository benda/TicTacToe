using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    class Cell : NotifyPropertyChangedEntity
    {
        private string _content;
        private const string Open = "-";
        public const string HumanMark = "X";
        public const string AIMark = "O";
        public int Value { get; set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Cell(int row, int column)
        {
            MarkOpen();
            Row = row;
            Column = column;
        }

        public void PlaceHuman()
        {
            Content = HumanMark;
        }

        public void PlaceAI()
        {
            Content = AIMark;
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOpen { get { return Content == Open; } }

        internal void MarkOpen()
        {
            Content = Open;
        }
    }
}
