using System;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class CameraDS:IComponent<ElementDS>
    {
        public Vector2 CameraFrame { get; set; }
        public Vector2 ViewFrame { get; set; }
        public float Size { get; set; }
        public string ID { get; set; }
    }
}
