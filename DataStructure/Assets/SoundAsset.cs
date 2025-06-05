using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.DataStructure.Assets
{
    public class SoundAsset : IAsset
    {
        public SoundAsset(string path)
        {
            Path = path;
        }
        public string Path { get; }
        public bool Relative { get; }
    }
}
