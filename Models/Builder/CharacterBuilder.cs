using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.AssetModels;
using System.IO;
using StoryMaker.Models.Components;

namespace StoryMaker.Models.Builder
{
    public class CharacterBuilder
    {
        public StoryObject Build(DrawableAssetModel asset, Transform parent = null)
        {
            var character = new StoryObject()
            {
                Name = Path.GetFileNameWithoutExtension(asset.FilePath)
            };

            character.transform.SetParent(parent);


            var imageComponent = character.AddComponent<ImageRenderer>();
            imageComponent.ChangeImage(asset);

            return character;
        }
    }
}
