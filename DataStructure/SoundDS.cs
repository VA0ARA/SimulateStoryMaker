using System;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class SoundDS:IComponent<ElementDS>
    {
        public SoundAsset SoundAsset;
        public bool PlayOnAwake;
        public bool Loop;
        public float Delay;
        public string ID { get; set; }
    }
}
