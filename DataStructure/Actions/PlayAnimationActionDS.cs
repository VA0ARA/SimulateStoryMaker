using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.DataStructure.Actions
{
    public class PlayAnimationActionDS:IComponent<InteractivityDS>
    {
        public string ID, AnimatorID, ClipName;
        public float StartTime,Duration = 5;
       
    }
}
