using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure.Actions
{
    [Serializable]
    public class PlaySoundDS: IComponent<InteractivityDS>
    {
        public SoundAsset SoundAsset;
        public float StartTime,Duration;
        public string ID { get; set; }
    }
}
