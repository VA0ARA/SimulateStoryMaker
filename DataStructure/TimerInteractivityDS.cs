using System;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class TimerInteractivityDS:InteractivityDS
    {
        public float Time;
        public TimerInteractivityDS(float time) : base(TriggerType.Timer)
        {
            Time = time;
        }
    }
}
