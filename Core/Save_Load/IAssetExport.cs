using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Core.Save_Load
{
    public interface IAssetExport
    {
        void Export(string mainPath, IEnumerable<string> assetPaths);
        void Export(string mainPath, IEnumerable<string> assetPaths,out IEnumerable<AssetReplacementInfo> replacedAssets);
    }
}
