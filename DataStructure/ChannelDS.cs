using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class ChannelDS
    {
        public Type RecordedObjectType { get; set; }

        public string ObjectAddress { get; set; }

        public string MemberName { get; set; }

        public string ID { get; set; }

        public List<KeyValueDS> KeyValues { get; set; } =
            new List<KeyValueDS>();

        public static float GetChannelDuration(ChannelDS channel)
        {
            return channel.KeyValues.Max(k => k.Key)/30f;
        }
    }
}
