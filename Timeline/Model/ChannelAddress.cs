using System;

namespace Timeline.Model
{
    public readonly struct ChannelAddress
    {
        public string ObjectAddress { get; }
        public string MemberName { get; }
        public Type Type { get; }

        public ChannelAddress(string objectAddress, string memberName, Type type)
        {
            ObjectAddress = objectAddress;
            MemberName = memberName;
            Type = type;
        }
    }
}