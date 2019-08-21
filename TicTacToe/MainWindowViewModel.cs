using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class MainWindowViewModel : NotifyPropertyChangedEntity
    {
        private AutoResetEvent _humanMoveEvent = new AutoResetEvent(false);
        private Player _human;
        private Player _ai;
        private Player _activePlayer;
        private Thread _gameLoopThread;
        public Board Board { get; private set; }
        private string _status;
        private bool _easy;
        private bool _medium;
        private bool _hard = true;
        private object _lock = new object();

        public MainWindowViewModel()
        {
            SetupNewGame();

            _gameLoopThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    _ai.Easy = Easy;
                    _ai.Medium = Medium;
                    _ai.Hard = Hard;

                    _activePlayer.NextMove();

                    GameState state = Board.GetCurrentGameState();
                    if (state != GameState.InProgress)
                    {
                        Status = "Game Over: " + state;
                        Thread.Sleep(3000);
                        SetupNewGame();
                    }
                    else
                    {
                        _activePlayer = _activePlayer.IsHuman ? _ai : _human;
                    }
                }
            }));
            _gameLoopThread.IsBackground = true;
            _gameLoopThread.Start();
        }

        private void SetupNewGame()
        {
            lock (_lock)
            {
                GameState endingState = Board != null ? Board.GetCurrentGameState() : GameState.Draw;
                Board = new Board();
                _human = new Player(true, _humanMoveEvent, Board);
                _ai = new Player(false, _humanMoveEvent, Board);
                _activePlayer = endingState == GameState.HumanWin ? _ai : _human;
                Status = "Active";
                RaisePropertyChanged("Board");
            }
        }

        internal void NextHumanMove(Cell p)
        {
            lock (_lock)
            {
                if (_activePlayer.IsHuman && p.IsOpen)
                {
                    _human.SetNextMove(p);
                }
            }
        }

        public bool Easy
        {
            get { return _easy; }
            set
            {
                _easy = value;
                if (value)
                {
                    Medium = !value;
                    Hard = !value;
                }
                RaisePropertyChanged();
            }
        }

        public bool Medium
        {
            get { return _medium; }
            set
            {
                _medium = value;
                if (value)
                {
                    Easy = !value;
                    Hard = !value;
                }
                RaisePropertyChanged();
            }
        }

        public bool Hard
        {
            get { return _hard; }
            set
            {
                _hard = value;
                if (value)
                {
                    Medium = !value;
                    Easy = !value;
                }
                RaisePropertyChanged();
            }
        }


        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

    }
}
