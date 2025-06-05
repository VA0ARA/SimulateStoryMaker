using System;
using System.Linq;
using StoryMaker.Helpers;
using System.Collections.Generic;

namespace Timeline.Model
{
    /// <summary>
    /// This struct Reperesent the member(field) which is recorded
    /// </summary>
    public class Channel : KeyValues
    {
        public string ObjectAddress { get; set; }

        public string MemberName { get; set; }

        public Type Type { get; set; }


        public Channel(string objectAddress, string memberName , Type type)
        {
            ObjectAddress = objectAddress;
            MemberName = memberName;
            Type = type;
        }

        public override string ToString()
        {
            var address = ObjectAddress;
            if (address.Contains("/"))
                address = address.Substring(address.LastIndexOf('/') + 1);

            return $"{address} : {MemberName}";
        }
    }
}