using System;

namespace Timeline.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class NonRecordable : System.Attribute
    {
    }
}