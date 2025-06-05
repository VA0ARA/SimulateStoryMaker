using System;
using System.Threading.Tasks;
using StoryMaker.Models.Interfaces;
using System.ComponentModel;
using StoryMaker.Models.Components;
using System.Linq;

namespace StoryMaker.Models.Actions
{
    public class PlayAnimationAction : IAction, INotifyPropertyChanged,IStop
    {
        //private Animator _animator;

        private string _animatorID,_clipName;

        public string AnimatorID
        {
            get => _animatorID;
            set
            {
                if(_animatorID==value)
                    return;

                _animatorID = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(AnimatorID)));
            }
        }


        public string ClipName
        {
            get => _clipName;
            set
            {
                if(_clipName==value)
                    return;

                _clipName = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(ClipName)));
            }
        }
        private float _startTime = 0;

        public float StartTime
        {
            get { return _startTime; }

            set
            {
                if (_startTime == value) return;

                _startTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartTime)));
            }
        }

        private float _duration;

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

        private bool _executing;

        public event PropertyChangedEventHandler PropertyChanged;
        public string ID { get; }
        //public PlayAnimationAction(string id,Animator animator, Timeline.Model.Clip clip)
        //{
        //    _animator = animator;
        //    ClipName = clip;
        //    Duration = (float) clip.GetLastFrame() / 30;
        //    this.ID = id;
        //    AnimatorID = animator.ID;
        //}

        public PlayAnimationAction()
        {
            ID = Guid.NewGuid().ToString();
        }
        public PlayAnimationAction(string id)
        {
            this.ID = id;
            //var animator = LoadAnimator();
            //var clip = LoadClip(animator);
            //Duration=(float) clip.GetLastFrame() / 30;
        }

        public async void Execute()
        {
            if (_executing)
                return;

            _executing = true;
            var animator = StoryObject.GetComponentByID<Animator>(AnimatorID);
            
            await Task.Delay(TimeSpan.FromSeconds(StartTime));
            animator.CurrentClipName = ClipName;
            if (_executing)
                await Task.Delay(TimeSpan.FromSeconds(Duration));

            animator.Stop();
            _executing = false;
        }

        private Timeline.Model.Clip LoadClip(Animator animator)
        {

            var clips=animator.AnimatorController.Clips;
            return clips.Single(c => c.Name == ClipName);
        }

        public void Stop()
        {
            _executing = false;
        }
    }
} 