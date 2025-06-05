using System;
using System.Timers;

namespace GameEngine
{
    public class Time
    {
        private readonly Timer _timer;

        private DateTime _lastTime;

        private static float _currentTime = 0;

        public static float CurrentTime
        {
            get { return _currentTime; }
            protected set
            {
                if (_currentTime.Equals(value)) return;
                _currentTime = value;
                CurrentMillSec = (int) (_currentTime * 1000);
            }
        }

        // this can will change in inherited class
        public static float DeltaTime { get; protected set; }

        private static int _currentMillSec = 0;

        private static int CurrentMillSec
        {
            get { return _currentMillSec; }
            set
            {
                if (_currentMillSec.Equals(value)) return;
                _currentMillSec = value;
                CurrentTime = (float) CurrentMillSec / 1000;
            }
        }

        public static event Action OnTimeChanged;
        
        public Time()
        {
            _timer = new Timer(1);
        }

        protected void StartTime(float startTime)
        {
            CurrentTime = startTime;
            _timer.Elapsed += _timer_Elapsed;

            _timer.Start();
            _timer.Enabled = true;
        }

        protected void PauseTime()
        {
            _timer.Enabled = false;
        }

        protected void ResumeTime()
        {
            _timer.Enabled = true;
        }

        protected void StopTime()
        {
            _timer.Elapsed -= _timer_Elapsed;
            _timer.Enabled = false;
            _timer.Stop();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var elapsed = (e.SignalTime - _lastTime).Milliseconds;
            CurrentMillSec += elapsed;
            _lastTime = e.SignalTime;
            OnTimeChanged?.Invoke();
        }
    }
}