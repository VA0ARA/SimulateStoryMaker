using System.Collections.Generic;

namespace StoryMaker.Models.Interfaces
{
    public interface IHierarchyNode
    {
        string Name { get; }
        IEnumerable<IHierarchyNode> Nodes { get; }

        bool IsExpanded { get; set; }
    }
}