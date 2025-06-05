using StoryMaker.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Models.Components.Interactivities
{
    public class Timer : InterActivity
    {
        public Timer()
        {
            ID = System.Guid.NewGuid().ToString();
        }

        public Timer(string id)
        {
            this.ID = id;
        }


        float _startTime, _totalTime;
        bool _executed;
        public float StartTime
        {
            get => _startTime;
            set
            {
                if(_startTime!=value)
                {
                    _startTime = value;
                    RaisePropertyChanged(nameof(StartTime));
                }
            }
        }
        public override TriggerType GetTriggerType()
        {
            return TriggerType.Timer;
        }

        void Update()
        {
            _totalTime += GameEngine.Time.DeltaTime;
            if (!_executed && _totalTime>_startTime)
            {

                _executed = true;
                Execute();
            }
        }

        void Start()
        {
            _executed = false;
            _totalTime = 0;
        }

        protected override void Pause()
        {
            base.Pause();
        }
    }
}
