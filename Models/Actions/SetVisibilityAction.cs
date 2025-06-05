using System.ComponentModel;
using StoryMaker.Models.Interfaces;
using System;
using System.Linq;
using StoryMaker.Models.Components;

namespace StoryMaker.Models.Actions
{
    public class SetVisibilityAction : IAction, INotifyPropertyChanged,IStop
    {
        private float _startTime;
        public float StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value) return;

                _startTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartTime)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _visibility,_executing,_oldVisibility;
        public bool Visibility
        {
            get { return _visibility; }

            set
            {
                _visibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
            }
        }
        public string ID { get; }
        string _imageID;
        public string ImageID
        {
            get => _imageID;
            set
            {
                if(_imageID!=value)
                {
                    _imageID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageID)));
                }
            }
        }
        public float Duration { get; set; }

        public SetVisibilityAction()
        {
            ID = Guid.NewGuid().ToString();
        }
        public SetVisibilityAction(string id)
        {
            this.ID = id;
        }

        public void Stop()
        {
            _executing = false;
            try
            {
                var img = StoryObject.GetObjectsOfType<ImageRenderer>().Single(i => i.ID == ImageID);
                img.Visibility = _oldVisibility;
            }
            catch 
            {
                return;
            }


        }

        public async void Execute()
        {
            if (_executing)
                return;

            _executing = true;
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(StartTime));
            if (_executing)
            {
                var img = StoryObject.GetObjectsOfType<ImageRenderer>().Single(i => i.ID == ImageID);
                _oldVisibility = img.Visibility;
                img.Visibility = Visibility;
                await System.Threading.Tasks.Task.Delay((int)(Duration * 1000));
            }
            Stop();
        }
    }
}