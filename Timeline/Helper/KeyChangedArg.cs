using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Editor.Utility;
using StoryMaker.Helpers;
using Timeline.Model;

namespace Timeline.Helper
{
    public class KeyChangedArg
    {
        public ChannelAddress Address { get; }
        public KeyValue OldKey { get; }

        public int NewFrame
        {
            get;
        }
        public KeyChangedArg(ChannelAddress address, KeyValue oldKey,int newFrame)
        {
            Address = address;
            OldKey = oldKey;
            NewFrame = newFrame;
        }
    }
}
