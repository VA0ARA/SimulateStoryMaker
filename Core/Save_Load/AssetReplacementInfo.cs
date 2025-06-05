using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using StoryMaker.Models;

namespace StoryMaker.Core.Save_Load
{
    public class AssetReplacementInfo
    {
        public AssetReplacementInfo(string oldAssetName,string newAssetName)
        {
            this.OldAssetName =NormalizePath.Normalize(AssetsController.GetLocalAssetPath(oldAssetName));
            this.NewAssetName =NormalizePath.Normalize(AssetsController.GetLocalAssetPath(newAssetName));
        }
        public string NewAssetName { get; }
        public string OldAssetName { get; }
    }
}
