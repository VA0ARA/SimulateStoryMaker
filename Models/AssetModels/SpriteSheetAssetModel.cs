using System;

namespace StoryMaker.Models.AssetModels
{
    public class SpriteSheetAssetModel : DrawableAssetModel
    {
        public SpriteDetail SpriteDetail { get; protected set; }

        public SpriteSheetAssetModel(string path ) : base(path )
        {
            SpriteDetail = new SpriteDetail(this);
        }
    }
}