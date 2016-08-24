using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Log;

namespace Tetris
{
    public abstract class GameEngine
    {
        public bool IsRunning { get; private set; }
        private bool runNextMoveLoop;
        private Task GameEngineNextMoveTask;
        private readonly ManualResetEvent _manualResetEvent;
        private int _gameSpeed;


        public int GameSpeed
        {
            get { return _gameSpeed; }
            set
            {
                if (GameSpeed >= 0 && GameSpeed <= 100)
                    _gameSpeed = value;
            }
        }

        protected GameEngine()
        {
            _manualResetEvent = new ManualResetEvent(false);
        }
        
        public bool Start()
        {
            if (IsRunning || GameEngineNextMoveTask != null)
                return false;

            IsRunning = true;
            runNextMoveLoop = true;
            GameEngineNextMoveTask = new Task(RunGameThread);
            GameEngineNextMoveTask.Start();

            return true;
        }
        public void Stop()
        {
            if (IsRunning)
            {
                runNextMoveLoop = false;
                _manualResetEvent.Set();
            }
        }

        public abstract void NextMovement();
        public abstract void MoveLeft();
        public abstract void MoveRight();
        public abstract void RotateCW();
        public abstract void RotateCCW();
        public abstract void OnPaint(PaintEventArgs e);

        private void RunGameThread()
        {
            while (runNextMoveLoop)
            {
                try
                {
                    NextMovement();
                }
                catch (Exception ex)
                {
                    LogWriter.LogError("Error running NextMovement", ex);
                }
                
                _manualResetEvent.Reset();
                if (_gameSpeed == 0)
                    _manualResetEvent.WaitOne(-1);
                else
                {
                    _manualResetEvent.WaitOne(1 / _gameSpeed * 1000);
                }
            }

            IsRunning = false;
            GameEngineNextMoveTask = null;
        }
    }
}
