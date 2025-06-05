using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GameEngine;
using StoryMaker.Editor.Utility;
using Timeline.Attribute;
using Timeline.Helper;
using Timeline.Model;
using Timeline.Model.Interfaces;

namespace StoryMaker.Models.Components
{
    public sealed class Animator : Component
    {
        #region Properties

        public Animator()
        {
            ID = Guid.NewGuid().ToString();
        }

        public Animator(string id)
        {
            this.ID = id;
        }
        private bool _applyRootMotion;

        [NonRecordable]
        public bool ApplyRootMotion
        {
            get { return _applyRootMotion; }
            set
            {
                if (_applyRootMotion == value) return;
                _applyRootMotion = value;

                RaisePropertyChanged(nameof(ApplyRootMotion));
            }
        }

        private bool _playOnAwake;

        [NonRecordable]
        public bool PlayOnAwake
        {
            get { return _playOnAwake; }
            set
            {
                if (_playOnAwake == value) return;
                _playOnAwake = value;

                RaisePropertyChanged(nameof(PlayOnAwake));
            }
        }

        // First key is story object ,second key is channel
        // After all in KeyValue key is instance object type and MemberInfo
        private readonly Dictionary<StoryObject, Dictionary<Channel, KeyValuePair<object, PropertyInfo>>>
            _memberInfos = new Dictionary<StoryObject, Dictionary<Channel, KeyValuePair<object, PropertyInfo>>>();
        
        private Map<string, StoryObject> _objectsMap = new Map<string, StoryObject>();

        private AnimatorController _animatorController = new AnimatorController();

        public AnimatorController AnimatorController
        {
            get
            {
                return _animatorController;
            }
            set
            {
                if (_animatorController == value) return;
                _animatorController = value;

                RaisePropertyChanged(nameof(AnimatorController));
            }
        }

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        private float _currentTime = 0;

        private Clip _currentClip;

        private int _lastKeyframe;

        private string _currentClipName;

        public string CurrentClipName
        {
            get { return _currentClipName; }
            set
            {
                if (_currentClipName == value) return;
                _currentClipName = value;

                var clip = AnimatorController?.Clips.FirstOrDefault(c => c.Name == value);
                Stop();
                _currentClip = clip;
                if (clip != null)
                    PlayClip(clip.Name);
                RaisePropertyChanged(nameof(CurrentClipName));
            }
        }
        #endregion

        #region Public Methods

        public void PlayClip(string clipName)
        {
            var clip = AnimatorController?.Clips.FirstOrDefault(c => c.Name == clipName);
            if (clip == null)
            {
                throw new WarningException($"{clipName} there is not in animator clips");
            }

            _lastKeyframe = clip.GetLastFrame();
            _currentClip = clip;
            Play();
        }

        public void Play()
        {
            if (_currentClip == null) return;
            _isPlaying = true;
        }

        public void Pause()
        {
            _isPlaying = false;
        }

        public void Stop()
        {
            _isPlaying = false;
            _currentTime = 0;
        }

        private void Awake()
        {
            _objectsMap = AnimationUtility.GetObjectMap(this);
            CollectProperties();
        }

        private void Start()
        {
            if (!PlayOnAwake ||  AnimatorController.DefaultClip == null) return;
            PlayClip(AnimatorController.DefaultClip.Name);
        }

        private void Update()
        {
            if (_currentClip == null) return;
            if (!IsPlaying) return;
            _currentTime += Time.DeltaTime;

            var currentFrame = _currentTime * 30;

            if (currentFrame > _lastKeyframe)
            {
                _currentTime -= (float)_lastKeyframe / 30;
                currentFrame = _currentTime * 30;
            }

            foreach (var channel in _currentClip.Channels)
            {
                if (!_objectsMap.Contains(channel.ObjectAddress)) continue;
                var obj = _objectsMap[channel.ObjectAddress];
                
                if (ApplyRootMotion && obj == StoryObject) continue;
                
                IValueAtTime valueAtTime = channel;
                var value = valueAtTime.GetValue(currentFrame);
                
                var memberInfoData = _memberInfos[obj][channel];
                memberInfoData.Value?.SetValue(memberInfoData.Key, value);
            }
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponent<Animator>() == null;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }
        #endregion

        #region Animator Utility

        protected override void StoryObjectAssigned()
        {
        }

        protected override void StoryObjectReleased()
        {
        }

        private void CollectProperties()
        {
            _memberInfos.Clear();

            for (var i = 0; i < AnimatorController.Clips.Count; i++)
            {
                for (var j = 0; j < AnimatorController.Clips[i].Channels.Count; j++)
                {
                    var channel = AnimatorController.Clips[i].Channels[j];
                    if (!_objectsMap.Contains(channel.ObjectAddress)) continue;
                    var obj = _objectsMap[channel.ObjectAddress];

                    var component = obj.GetComponentByType(channel.Type);

                    var property = component?.GetType().GetProperty(channel.MemberName);


                    if (_memberInfos.ContainsKey(obj))
                    {
                        if (!_memberInfos[obj].ContainsKey(channel))
                            _memberInfos[obj].Add(channel, new KeyValuePair<object, PropertyInfo>(component, property));
                    }
                    else
                    {
                        var memberInfo = new Dictionary<Channel, KeyValuePair<object, PropertyInfo>>
                        {
                            {channel, new KeyValuePair<object, PropertyInfo>(component, property)},
                        };
                        _memberInfos.Add(obj, memberInfo);
                    }
                }
            }
        }
        #endregion
    }
}