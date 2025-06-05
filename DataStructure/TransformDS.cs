using System;

namespace StoryMaker.DataStructure
{
    /// <summary>
    /// This class stores position ,rotation ,scale ,pivote & parent of an element
    /// </summary>
    [Serializable]
    public class TransformDS:IComponent<ElementDS>
    {
        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }
        public Vector2 LocalScale { get; set; }
        //TODO Pivot
        // public Vector2 Pivot { get; set; }
        public TransformDS Parent { get; set; }
        public string ID { get; set; }
    }
}
