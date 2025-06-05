using System;

namespace StoryMaker.Models.Interfaces
{
    public interface IOrder
    {
        int GroupOrder { get; }
        int SortOrder { get; }
        int UniqueOrder { get; }
    }
}