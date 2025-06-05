using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = StoryMaker.Models;

namespace StoryMaker.Core
{
    public class ElementBoundary : IBoundary
    {
        Models.StoryObject _storyObject;
        public ElementBoundary(Models.StoryObject storyObject)
        {
            _storyObject = storyObject;
        }
        public Bounds? GetBounds()
        {
            var image = _storyObject.GetComponent<Models.Components.ImageRenderer>();
            if (image == null)
                return null;
            var pos = image.StoryObject.transform.Position;
            IntPoint position = new IntPoint((int)pos.X, (int)pos.Y);
            var size = image.DrawableBound;
            return new Bounds(position.X, position.Y, (int)size.X, (int)size.Y);
        }
    }
}
