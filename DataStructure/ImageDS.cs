using System;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class ImageDS : IComponent<ElementDS>
    {
        public bool Visibility { get; set; }
        public ImageAsset ImageAsset { get; set; }

        public int GroupOrder { get; set; }
        public int SortOrder { get; set; }
        public Vector2 Pivot;
        public string ID { get; set; }
        public bool FlipX, FlipY;
    }
}
