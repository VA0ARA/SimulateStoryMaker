using System;
using System.ComponentModel;
using System.Windows;
using StoryMaker.Helpers;
using StoryMaker.Models.Interfaces;
using Timeline.Attribute;

namespace StoryMaker.Models.Components
{
    public sealed class Camera : Component, IDrawable
    {
        private Vector2 _cameraFrame = new Vector2(856, 480);

        [NonRecordable]
        public Vector2 CameraFrame
        {
            get { return _cameraFrame; }

            set
            {
                if (_cameraFrame.Equals(value))
                    return;

                _cameraFrame = new Vector2((int) value.X, (int) value.Y);
                RaisePropertyChanged(nameof(CameraFrame));
                RaisePropertyChanged(nameof(DrawableBound));
            }
        }

        private Vector2 _viewFrame = new Vector2(856, 480);

        [NonRecordable]
        public Vector2 ViewFrame
        {
            get
            {
                _viewFrame = new Vector2(CameraFrame.X * Size * (1 / Math.Abs(StoryObject.transform.Scale.X)),
                    CameraFrame.Y * Size * (1 / Math.Abs(StoryObject.transform.Scale.Y)));
                return _viewFrame;
            }
        }

        private float _size = 1;

        public float Size
        {
            get { return _size; }
            set
            {
                if (_size.Equals(value)) return;
                _size = value;
                RaisePropertyChanged(nameof(Size));
                RaisePropertyChanged(nameof(DrawableBound));
            }
        }

        #region Constructor

        public Camera()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public Camera(string id)
        {
            this.ID = id;
        }

        protected override void StoryObjectAssigned()
        {
            StoryObject.transform.PropertyChanged += Scale_PropertyChanged;
        }

        // Raise ViewFrame property changed to determinate viewFrame to ignore scale
        private void Scale_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StoryObject.transform.Scale))
                RaisePropertyChanged(nameof(DrawableBound));
        }

        #endregion

        #region Method

        public Vector2 ScreenPointToWorld(Point mousePosition)
        {
            return new Vector2(
                (float) (StoryObject.transform.Position.X + mousePosition.X * StoryObject.transform.Scale.X),
                (float) (StoryObject.transform.Position.Y + mousePosition.Y * StoryObject.transform.Scale.Y));
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponent<Camera>() == null && storyObject.Drawable == null;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return storyObject.GetComponent<Camera>() == null;
        }

        #endregion

        #region IDrawable
        public Vector2 DrawableBound => ViewFrame;

        private readonly Vector2 _pivot = new Vector2(0.5f,0.5f);
        public Vector2 Pivot => _pivot;

        #endregion
        
        public int GroupOrder => int.MaxValue;
        
        public int SortOrder => int.MaxValue;
        
        public int UniqueOrder
        {
            get { return int.MaxValue; }
        }
    }
}