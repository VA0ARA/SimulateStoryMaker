using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.DataStructure.Actions
{
    [Serializable]
    public class SetVariableDS: IComponent<InteractivityDS>
    {
        public bool Increase;
        public VariableDS Parameter1, Parameter2;
        public float StartTime;
        public string ID { get; set; }

    }
}
