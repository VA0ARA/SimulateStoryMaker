using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StoryMaker.Helpers;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Interfaces;
using Timeline.Attribute;

namespace StoryMaker.Models.Components
{
    public class ImageRenderer : Component, IDrawable, IAssetModelCollector
    {
        private DrawableAssetModel _currentAsset;

        public DrawableAssetModel CurrentAsset
        {
            get { return _currentAsset; }
            set
            {
                if (_currentAsset != null && _currentAsset.Equals(value)) return;
                _currentAsset = value;

                switch (value)
                {
                    case SpriteSheetAssetModel spriteSheet:
                        CurrentImage = spriteSheet.SpriteDetail.GetImage(0);
                        break;
                    default:
                        CurrentImage = new SpriteDetail(value).GetImage(0);
                        break;
                }

                RaisePropertyChanged(nameof(CurrentAsset));
            }
        }

        private CustomImage _currentImage;

        [NonRecordable]
        public CustomImage CurrentImage
        {
            get { return _currentImage; }

            private set
            {
                if (_currentImage != null && _currentImage.Equals(value)) return;
                _currentImage = value;

                RaisePropertyChanged(nameof(CurrentImage));
                RaisePropertyChanged(nameof(DrawableBound));
            }
        }

        private Vector2 _pivot;

        public Vector2 Pivot
        {
            get { return _pivot; }

            set
            {
                if (_pivot.Equals(value)) return;

                _pivot = value;
                RaisePropertyChanged(nameof(Pivot));
            }
        }

        private bool _flipX = false;
        public bool FlipX
        {
            get => _flipX;
            set
            {
                if(_flipX == value) return;
                _flipX = value;
                RaisePropertyChanged(nameof(FlipX));
            }
        }

        private bool _flipY = false;
        public bool FlipY
        {
            get => _flipY;
            set
            {
                if (_flipY == value) return;
                _flipY = value;
                RaisePropertyChanged(nameof(FlipY));
            }
        }

        private bool _visibility = true;

        public bool Visibility
        {
            get { return _visibility; }

            set
            {
                if (_visibility.Equals(value)) return;
                _visibility = value;
                RaisePropertyChanged(nameof(Visibility));
            }
        }

        private int _groupOrder;
        public int GroupOrder
        {
            get { return _groupOrder; }
            set
            {
                if(_groupOrder == value) return;
                _groupOrder = value;
                RaisePropertyChanged(nameof(GroupOrder));
                RaisePropertyChanged(nameof(UniqueOrder));
            }
        }


        private int _sortOrder;

        public int SortOrder
        {
            get { return _sortOrder; }

            set
            {
                if (_sortOrder.Equals(value)) return;
                _sortOrder = value;
                RaisePropertyChanged(nameof(SortOrder));
                RaisePropertyChanged(nameof(UniqueOrder));
            }
        }
        public int UniqueOrder
        {
            get
            {
                if (GroupOrder == 0) return SortOrder;

                var wholeNumber = short.MaxValue + SortOrder;
                var wholeNumberText = wholeNumber.ToString();
                var wholeNumberLength = wholeNumberText.Length;
                var missingZero = int.MaxValue.ToString().Length / 2 - wholeNumberLength;

                for (var i = 0; i < missingZero; i++)
                {
                    wholeNumberText = wholeNumberText.Insert(0, "0");
                }

                return int.Parse(GroupOrder + wholeNumberText);
            }
        }

        #region Constructor

        public ImageRenderer()
        {
            Pivot = new Vector2(0.5f, 0.5f);
            Visibility = true;
            ID = System.Guid.NewGuid().ToString();
        }


        public ImageRenderer(string id)
        {
            Pivot = new Vector2(0.5f, 0.5f);
            Visibility = true;
            ID = id;
        }

        #endregion

        #region Methods

        public void ChangeImage(DrawableAssetModel asset)
        {
            CurrentAsset = asset;
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponents<Component>().All(c => !(c is IDrawable));
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }

        #endregion

        #region IAssetCollector

        public IEnumerable<AssetModel> GetAssets()
        {
            return new List<AssetModel>() {CurrentAsset};
        }

        public IEnumerable<IAssetModelCollector> GetCollectors()
        {
            return null;
        }

        #endregion

        #region IDrawable
        public Vector2 DrawableBound
        {
            get
            {
                if (CurrentImage == null) return new Vector2();
                if (CurrentImage.Value is BitmapImage img)
                {
                    return new Vector2(img?.PixelWidth ?? 0, img?.PixelHeight ?? 0);
                }

                var image = ImageHelper.GetImage(CurrentImage.Value.ToString(), BitmapScalingMode.Linear);
                return new Vector2(image?.PixelWidth ?? 0, image?.PixelHeight ?? 0);
            }
        }

        #endregion
    }
}