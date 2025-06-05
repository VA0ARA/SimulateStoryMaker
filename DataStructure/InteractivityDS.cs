using System;
using System.Collections.Generic;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class InteractivityDS:IComponent<ElementDS>
    {
        public TriggerType TriggerType { get; }
        public List<IComponent<InteractivityDS>> Actions = new List<IComponent<InteractivityDS>>();
        public string ID { get; set; }

        public InteractivityDS(TriggerType triggerType)
        {
            TriggerType = triggerType;
        }
    }
}
