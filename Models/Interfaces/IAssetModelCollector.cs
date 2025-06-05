using System.Collections.Generic;
using StoryMaker.Models.AssetModels;

namespace StoryMaker.Models.Interfaces
{
    public interface IAssetModelCollector
    {
        IEnumerable<AssetModel> GetAssets();
        //IEnumerable<IAssetModelCollector> GetCollectors();
    }
}