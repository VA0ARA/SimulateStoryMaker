using System;
using System.Collections.Generic;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class AnimatorControllerDS
    {
        public ClipDS DefaultClip { get; set; }
        public List<ClipDS> Clips { get; set; } = new List<ClipDS>();
    }
}