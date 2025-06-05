using System.Windows.Input;
using StoryMaker.DataStructure;
using System;
using Core = StoryMaker.Core;

namespace StoryMaker.Models.Components.Interactivities
{
    public class Drag : InterActivity,Core.Drag_Drop.IDrag
    {
        public Drag()
        {
            ID = Guid.NewGuid().ToString();
        }
        public Drag(string id)
        {
            this.ID = id;
        }

        private System.Windows.Point _mousePos;
        private Vector2 CharPos;
        public bool AllowDrop = true;

        public event EventHandler DragStarted;
        public event EventHandler DragFinished;

        public bool Dragging { get; private set; }

        public override void MouseDown()
        {
            _mousePos = Mouse.GetPosition(null);
            CharPos = StoryObject.transform.Position;
            Dragging = true;
            DragStarted?.Invoke(this, null);
            Execute();
        }

        public override void MouseDrag()
        {
            var delta = Mouse.GetPosition(null) - _mousePos;
            StoryObject.transform.Position = new Vector2(CharPos.X + (float)delta.X, CharPos.Y + (float)delta.Y);
        }

        public override void MouseUp()
        {
            Dragging = false;
            DragFinished?.Invoke(this, null);
            Stop();
        }

        public override TriggerType GetTriggerType()
        {
            return TriggerType.Dragging;
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponent<Tap>() == null;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }
    }
}
