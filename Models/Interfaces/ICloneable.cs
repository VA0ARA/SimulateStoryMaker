using System;

namespace StoryMaker.Models.Interfaces
{
    public interface ICloneable<T> : ICloneable
    {
        void Paste(T t);
    }
}