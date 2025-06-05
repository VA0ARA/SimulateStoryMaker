using System;
using System.Collections.Generic;
using System.Windows;

namespace GameEngine
{
    public class Engine : Time
    {
        #region Static Variables

        // private static Engine _engine;
        //
        // public static Engine Singleton
        // {
        //     get
        //     {
        //         if (_engine == null)
        //         {
        //             _engine = new Engine();
        //         }
        //
        //         return _engine;
        //     }
        // }


        public EngineState EngineState
        {
            get { return _loopEngine.EngineState; }
        }

        public bool Pause
        {
            get { return _loopEngine.Pause; }
        }

        private readonly PortableLoopEngine _loopEngine;

        private bool _stepMode;
        public bool StepMode
        {
            get => _stepMode;
        }


        #endregion

        #region Constructor

        public Engine()
        {
            _loopEngine = new PortableLoopEngine(EndFrame);
            _loopEngine.DeltaTimeChanged += DeltaTimeChanged;
        }

        private void DeltaTimeChanged(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

        #endregion

        #region Methods

        private void EndFrame()
        {
            if (_stepMode)
                _loopEngine.PauseEngine();
        }

        public void StartEngine(List<object> objects, float startTime , bool realtime)
        {
            _loopEngine.StartEngine(objects ,realtime);
            StartTime(startTime);
        }

        public void PauseEngine()
        {
            _stepMode = true;
            PauseTime();
            _loopEngine.PauseEngine();
        }

        public void NextStep()
        {
            if(!_stepMode)
                PauseEngine();

            ResumeTime();
            _loopEngine.ResumeEngine();
        }

        public void ResumeEngine()
        {
            _stepMode = false;
            ResumeTime();
            _loopEngine.ResumeEngine();
        }

        public void StopEngine()
        {
            _stepMode = false;
            StopTime();
            _loopEngine.StopEngine();
        }

        #endregion
    }
}