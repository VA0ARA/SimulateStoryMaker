using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models;
using System.IO;
using Helpers;

namespace StoryMaker.Core.Save_Load
{
    public class AssetExport : IAssetExport
    {
        public void Export(string mainPath, IEnumerable<string> assetPaths)
        {
            foreach (var asset in assetPaths)
                ExportAsset(mainPath, asset);
        }

        public void Export(string mainPath, IEnumerable<string> assetPaths, out IEnumerable<AssetReplacementInfo> replacedAssets)
        {
            List<AssetReplacementInfo> replacedAssetsList = new List<AssetReplacementInfo>();
            replacedAssets = replacedAssetsList;
            foreach (string assetPath in assetPaths)
            {
                string fullPath = Path.Combine(mainPath,AssetsController.GetFilteredLocalAssetPath(assetPath));
                if (File.Exists(fullPath))
                {
                    string newName = NameSuggession.SuggestName(Path.GetFileNameWithoutExtension(assetPath), GetSimilarAvaiableFiles(fullPath)),
                        newAssetPath = Path.GetDirectoryName(assetPath)+"/" + newName + Path.GetExtension(fullPath);
                    replacedAssetsList.Add(new AssetReplacementInfo(assetPath, newAssetPath));
                    File.Move(assetPath, newAssetPath);
                    ExportAsset(mainPath, newAssetPath);
                }
                else
                    ExportAsset(mainPath, assetPath);
            }
        }

        IEnumerable<string> GetSimilarAvaiableFiles(string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(fileName));
            var availableFiles = dir.GetFiles($"{Path.GetFileNameWithoutExtension(fileName)}*{Path.GetExtension(fileName)}");
            return availableFiles.Select(f => Path.GetFileNameWithoutExtension(f.FullName));
        }

        void ExportAsset(string mainPath,string assetPath)
        {
            string localPath = AssetsController.GetLocalAssetPath(assetPath).TrimStart(new char[] { '/', '\\' });

            FileInfo newFileInfo = new FileInfo(Path.Combine(mainPath,localPath));
            if (newFileInfo.Exists)
                return;

            var directory = newFileInfo.Directory;
            if (!directory.Exists)
                directory.Create();

            File.Copy(assetPath, newFileInfo.FullName);
            
        }
    }
}
