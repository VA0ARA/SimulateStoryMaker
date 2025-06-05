using StoryMaker.Backend.Version2;
using DS=StoryMaker.DataStructure.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.VersionConverter
{
    public class AssetConverter : IAssetConverter
    {
        public DS.IAsset Convert(Asset asset)
        {
            switch(asset.FileType)
            {
                case FileTypes.Image:
                    return new DS.ImageAsset(asset.FileName);

                case FileTypes.Sound:
                    return new DS.SoundAsset(asset.FileName);

                case FileTypes.SpriteSheet:
                    return new StoryMaker.DataStructure.Assets.SpritesheetAsset(asset.FileName, asset.SpriteSheetMeta.Width,
                        asset.SpriteSheetMeta.Height, asset.SpriteSheetMeta.FPS);
            }

            throw new Exception($"The type of asset({asset.FileType} not supported!");
        }
    }
}
