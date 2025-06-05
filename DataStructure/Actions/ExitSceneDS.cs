using System;

namespace StoryMaker.DataStructure.Actions
{
    [Serializable]
    public class ExitSceneDS: IComponent<InteractivityDS>
    {
        public string ExitGate;
        public float StartTime;
        public string ID { get; set; }
    }
}
