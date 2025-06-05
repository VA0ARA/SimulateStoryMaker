using System;
using System.ComponentModel;
using StoryMaker.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Components;
using StoryMaker.Models.Interfaces;

namespace StoryMaker.Models.Actions
{
    public class ChangeImageAction : IAction, INotifyPropertyChanged, IAssetModelCollector, IStop
    {
        //private SpriteSheetPlayer _player = null;

        private float _duration = 0;
        public string ID { get; }

        //float _timer = 0;
        public float StartTime { get; set; }

        public float Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value) return;
                _duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
            }
        }

        private bool _executed = false;
        DrawableAssetModel _oldImage;

        public event PropertyChangedEventHandler PropertyChanged;

        private DrawableAssetModel _imageAsset;

        public DrawableAssetModel ImageAsset
        {
            get { return _imageAsset; }

            set
            {
                if (_imageAsset != value)
                {
                    _imageAsset = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageAsset)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
                }
            }
        }

        public string FileName
        {
            get
            {
                return ImageAsset!=null? ImageAsset.FilePath:"";
            }
        }

        string _imageID;
        public string ImageID
        {
            get => _imageID;
            set
            {
                if (_imageID == value)
                    return;

                _imageID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageID)));
            }
        }

        public ChangeImageAction()
        {
            ID = Guid.NewGuid().ToString();
        }
        public ChangeImageAction(string id)
        {
            this.ID = id;
        }

        public async void Execute()
        {
            if (_executed) return;
            _executed = true;
            var img = StoryObject.GetObjectsOfType<ImageRenderer>().Single(i => i.ID == ImageID);
            _oldImage = img.CurrentAsset;
            await Task.Delay((int)(StartTime*1000));
            if (_executed)
            {
                img.ChangeImage(ImageAsset);
                await Task.Delay((int)(Duration * 1000));
                img.ChangeImage(_oldImage);
                _executed = false;
            }
        }

        public void Stop()
        {
            if (!_executed)
                return;

            _executed = false;
            var img = StoryObject.GetObjectsOfType<ImageRenderer>().Single(i => i.ID == ImageID);
            if (img != null)
                img.ChangeImage(_oldImage);
        }

        public IEnumerable<AssetModel> GetAssets()
        {
            yield return ImageAsset;
        }
    }
}