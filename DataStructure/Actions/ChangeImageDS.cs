using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure.Actions
{
    [Serializable]
    public class ChangeImageDS:IComponent<InteractivityDS>
    {
        public ImageAsset ImageAsset;
        public float StartTime, Duration;//= new Asset(FileTypes.Image);
        public string ID { get; set; }
        public string ImageID;
    }
}
