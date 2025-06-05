using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryMaker.DataStructure
{
    /// <summary>
    /// This class stores the list of channels
    /// </summary>
    [Serializable]
    public class ClipDS
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public List<ChannelDS> Channels { get; set; } = new List<ChannelDS>();

        public static float GetClipDuration(ClipDS clip)
        {
            return clip.Channels.Max(c => ChannelDS.GetChannelDuration(c));
        }
    }
}
