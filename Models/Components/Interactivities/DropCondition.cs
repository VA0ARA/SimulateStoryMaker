using StoryMaker.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Models.Components.Interactivities
{
    public class DropCondition : InterActivity
    {
        public DropCondition()
        {
            ID = Guid.NewGuid().ToString();
        }

        public DropCondition(string id)
        {
            this.ID = id;
        }

        public DropCondition(string id,string draggingELementID,string dropElementID)
        {
            this.ID = id;
            this.DraggingElementID = draggingELementID;
            this.DropElementID = dropElementID;
        }

        protected override void Play()
        {
            base.Play();
        }

        protected override void Pause()
        {
            Console.WriteLine("Drop Condition Paused!!!!");
            base.Pause();
        }
        StoryObject _draggingElement, _dropElement;
        public StoryObject DraggingElement
        {
            get
            {
                if (_draggingElement == null && !string.IsNullOrEmpty(DraggingElementID))
                    _draggingElement = Reload(DraggingElementID);

                return _draggingElement;
            }
            set
            {
                if (_draggingElement != null && _draggingElement!=value)
                {
                    var drag = _draggingElement.GetComponent<Drag>();
                    if(drag!=null)
                        drag.DragFinished -= DropCondition_DragFinished;
                }
                if(_draggingElement!=value)
                {
                    _draggingElement = value;
                    var drag = _draggingElement.GetComponent<Drag>();
                    if (drag != null)
                        drag.DragFinished += DropCondition_DragFinished;

                    DraggingElementID = value?.ID;
                    
                    RaisePropertyChanged(nameof(DraggingElement));
                }
            }
        }

        StoryObject Reload(string id)
        {
            return StoryObject.GetStoryObjectByID(id);
        }

        public string DraggingElementID { get; private set; }
        public string DropElementID { get; private set; }

        private void DropCondition_DragFinished(object sender, EventArgs e)
        {
            if (_draggingElement == null || _dropElement == null)
                return;

            Core.Bounds? dragBounds = new Core.ElementBoundary(_draggingElement).GetBounds(),
                dropBounds = new Core.ElementBoundary(_dropElement).GetBounds();

            if (dragBounds == null || dropBounds == null)
                return;

            if (Core.Bounds.Intersects((Core.Bounds)dragBounds, (Core.Bounds)dropBounds))
                base.Execute();
        }

        public StoryObject DropElement
        {
            get
            {
                if (_dropElement == null && !string.IsNullOrEmpty(DropElementID))
                    _dropElement = Reload(DropElementID);

                return _dropElement;

            }
            set
            {
                if(_dropElement!=value)
                {
                    _dropElement = value;
                    DropElementID = value?.ID;
                    RaisePropertyChanged(nameof(DropElement));
                }
            }
        }
        public override TriggerType GetTriggerType()
        {
            return TriggerType.DropCondition;
        }
    }
}
