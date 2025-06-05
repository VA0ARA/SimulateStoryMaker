using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Backend.Version2;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.VersionConverter
{
    public interface IAssetConverter
    {
        IAsset Convert(Asset asset);
    }
}
