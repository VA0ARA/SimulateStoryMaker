using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Core.Drag_Drop
{
    public interface IDrag
    {
        bool Dragging { get; }
        event EventHandler DragStarted, DragFinished;
    }
}
