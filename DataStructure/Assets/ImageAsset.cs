using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.DataStructure.Assets
{
    public class ImageAsset : IAsset
    {
        public ImageAsset(string path)
        {
            Path = path;
        }
        public string Path { get; }
    }
}
