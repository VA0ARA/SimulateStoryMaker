using System;
using System.IO;
using System.Windows;
using StoryMaker.Helpers;
using System.Collections.Generic;

namespace StoryMaker.Models.AssetModels
{
    public class AssetModel : NotifyPropertyChange
    {

        public virtual string Name { get; protected set; }

        public string FilePath { get; protected set; }

        protected AssetModel(string filePath)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
        }

        public override bool Equals(object obj)
        {
            if (obj is AssetModel assetModel)
                return assetModel.FilePath.Equals(FilePath) && assetModel.GetType() == GetType();
            return false;
        }
    }
}