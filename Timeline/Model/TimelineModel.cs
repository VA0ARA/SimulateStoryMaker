using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameEngine;
using StoryMaker.Annotations;
using StoryMaker.Editor.Utility;
using StoryMaker.Models;
using StoryMaker.Models.Components;
using Swordfish.NET.Collections;
using Timeline.Attribute;
using Timeline.Helper;
using Timeline.Model.Interfaces;
using Component = StoryMaker.Models.Components.Component;

namespace Timeline.Model
{
    public class TimelineModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _framePerSecond = 30;

        public int FramePerSecond
        {
            get { return _framePerSecond; }
            set
            {
                if (_framePerSecond.Equals(value)) return;
                _framePerSecond = value;
                RaisePropertyChanged(nameof(FramePerSecond));
            }
        }

        private int _startFrame = 0;

        public int StartFrame
        {
            get { return _startFrame; }
            set
            {
                if (_startFrame.Equals(value)) return;
                _startFrame = value;
                RaisePropertyChanged(nameof(StartFrame));
            }
        }

        private int _endFrame = 3000;

        public int EndFrame
        {
            get { return _endFrame; }
            set
            {
                if (_endFrame.Equals(value)) return;

                _endFrame = value;
                RaisePropertyChanged(nameof(EndFrame));
            }
        }

        private float _currentFrame;

        public float CurrentFrame
        {
            get => _currentFrame;
            set
            {
                if (_currentFrame.Equals(value)) return;
                _currentFrame = value;

                // CurrentTime = (float) value / FramePerSecond;
                FrameChanged(_currentFrame);
                RaisePropertyChanged(nameof(CurrentFrame));
            }
        }

        private int _lastFrame;

        public int LastFrame
        {
            get => _lastFrame;
            private set
            {
                if (_lastFrame.Equals(value)) return;
                _lastFrame = value;

                RaisePropertyChanged(nameof(LastFrame));
            }
        }

        private readonly PortableLoopEngine _loopEngine;

        private RecordStatus _recordStatus = RecordStatus.Pause;

        public RecordStatus RecordStatus
        {
            get { return _recordStatus; }
            set
            {
                if (_recordStatus.Equals(value)) return;
                _recordStatus = value;

                if (value == RecordStatus.Play || value == RecordStatus.Record)
                    Preview = true;

                switch (value)
                {
                    case RecordStatus.Play:
                    {
                        _loopEngine.StartEngine(new List<object>() {this});
                        break;
                    }
                    case RecordStatus.Pause:
                    case RecordStatus.Stop:
                    case RecordStatus.Record:
                        _loopEngine.StopEngine();
                        break;
                }

                RaisePropertyChanged(nameof(RecordStatus));
            }
        }

        private bool _preview;

        public bool Preview
        {
            get { return _preview; }
            set
            {
                if (_preview == value) return;
                _preview = value;

                if (_preview == false)
                {
                    RecordStatus = RecordStatus.Stop;
                    UnTrack();
                    // SetComponentsToDefault();
                }
                else
                {
                    Track();
                    FrameChanged(CurrentFrame);
                }

                RaisePropertyChanged(nameof(Preview));
            }
        }

        public ObservableCollection<Animator> Animators { get; }

        private Animator _animator;

        public Animator Animator
        {
            get => _animator;
            set
            {
                if (_animator == value) return;
                Preview = false;
                _animator = value;
                RaisePropertyChanged(nameof(Animator));

                if (_animator == null) return;
                    RecordingClip = _animator.AnimatorController.Clips.FirstOrDefault();
            }
        }

        private Clip _recordingClip;

        public Clip RecordingClip
        {
            get => _recordingClip;
            set
            {
                if (_recordingClip == value) return;
                _recordingClip = value;

                if (_recordingClip != null)
                    LastFrame = _recordingClip.GetLastFrame();

                RaisePropertyChanged(nameof(RecordingClip));
            }
        }

        private Map<string, StoryObject> _objectsMap = new Map<string, StoryObject>();

        private KeyValuePair<Component, Component>[] _clonedComponent = new KeyValuePair<Component, Component>[0];

        private bool _frameChangeLock;

        public TimelineModel()
        {
            Animators = new ObservableCollection<Animator>();
            _loopEngine = new PortableLoopEngine();
        }

        public void ChangeAnimator(Animator animator)
        {
            Animator = animator;
        }

        private void Update()
        {
            if (RecordStatus != RecordStatus.Play) return;

            if (CurrentFrame >= EndFrame || CurrentFrame < StartFrame)
            {
                CurrentFrame = StartFrame;
            }
            else
            {
                CurrentFrame = (CurrentFrame / FramePerSecond + _loopEngine.DeltaTime) * FramePerSecond;
            }
        }

        private void Track()
        {
            _objectsMap = AnimationUtility.GetObjectMap(_animator);

            _clonedComponent = CloneComponents(_animator, _objectsMap.Set2);


            for (var i = 0; i < _clonedComponent.Length; i++)
            {
                _clonedComponent[i].Key.PropertyChanged += Component_OnPropertyChanged;
            }

            foreach (var storyObject in _objectsMap.Set2)
            {
                storyObject.PropertyChanged += StoryObject_PropertyChanged;
            }
        }

        private void UnTrack()
        {
            if (_objectsMap.Count > 0)
                foreach (var storyObject in _objectsMap.Set2)
                {
                    storyObject.PropertyChanged -= StoryObject_PropertyChanged;
                }

            if (_clonedComponent.Length > 0)
                for (var i = 0; i < _clonedComponent.Length; i++)
                {
                    _clonedComponent[i].Key.PropertyChanged -= Component_OnPropertyChanged;
                }

            SetComponentsToDefault();
            _objectsMap = new Map<string, StoryObject>();
            _clonedComponent = new KeyValuePair<Component, Component>[0];
        }

        private void SetComponentsToDefault()
        {
            if (_clonedComponent.Length > 0)
                for (var i = 0; i < _clonedComponent.Length; i++)
                {
                    _clonedComponent[i].Key.Paste(_clonedComponent[i].Value);
                }
        }

        public void AddNewClip(string name)
        {
            var clip = new Clip(name);
            _animator.AnimatorController.AddClip(clip);

            if (!_animator.AnimatorController.Clips.Contains(clip)) return;
            RecordingClip = clip;
        }

        public void FrameChanged(float frame)
        {
            if (!Preview)
                Preview = true;
            if (_recordingClip == null) return;
            _frameChangeLock = true;
            foreach (var channel in _recordingClip.Channels)
            {
                if (!_objectsMap.Contains(channel.ObjectAddress)) continue;
                var obj = _objectsMap[channel.ObjectAddress];

                if (_animator.ApplyRootMotion && obj == _animator.StoryObject) continue;

                IValueAtTime valueAtTime = channel;
                var value = valueAtTime.GetValue(frame);

                var component = obj.GetComponentByType(channel.Type);

                var property = component?.GetType().GetProperty(channel.MemberName);

                property?.SetValue(component, value);
            }

            _frameChangeLock = false;
        }

        private void StoryObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StoryObject.Name) ||
                e.PropertyName == nameof(StoryObject.transform.Children) ||
                e.PropertyName == nameof(StoryObject.Components))
            {
                if (!Preview) return;
                UnTrack();
                Track();
            }
        }

        private void Component_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if record status is on playing , must not raise property changed
            // this is optimization trick
            if (RecordStatus != RecordStatus.Record) return;
            // if (CanRecord?.Invoke() == false) return;
            if (_frameChangeLock) return;

            if (_recordingClip == null) return;

            if (!(sender is Component component) || component == _animator) return;

            var componentType = component.GetType();

            if (componentType.GetCustomAttribute(typeof(NonRecordable)) != null) return;

            var property = componentType.GetProperty(e.PropertyName);
            if (property == null || !property.CanWrite) return;
            if (property.GetCustomAttribute(typeof(NonRecordable)) != null) return;

            var objectAddress = _objectsMap[component.StoryObject];

            CaptureProperty(new ChannelAddress(objectAddress, e.PropertyName, componentType));
        }

        public void CaptureProperty(ChannelAddress channelAddress)
        {
            if (!_objectsMap.Contains(channelAddress.ObjectAddress)) return;
            var component = _objectsMap[channelAddress.ObjectAddress].Components
                .FirstOrDefault(c => c.GetType() == channelAddress.Type);

            _recordingClip.AddKeyValue(channelAddress, (int) CurrentFrame,
                channelAddress.Type.GetProperty(channelAddress.MemberName)?.GetValue(component));
            LastFrame = _recordingClip.GetLastFrame();
        }

        public void AddKey(
            IEnumerable<KeyValuePair<ChannelAddress, KeyValuePair<int, object>>> data)
        {
            _recordingClip.AddKeyValue(data);
            LastFrame = _recordingClip.GetLastFrame();
        }

        public void DeleteKey(IEnumerable<KeyValue> keyValues)
        {
            _recordingClip.DeleteKey(keyValues);
            LastFrame = _recordingClip.GetLastFrame();
        }

        public void MoveKey(IEnumerable<Tuple<KeyValue, int>> data)
        {
            _recordingClip.Move(data);
            LastFrame = _recordingClip.GetLastFrame();
        }

        public bool IsComponentRecording(Component component)
        {
            return RecordStatus == RecordStatus.Record &&
                   _clonedComponent.Select(c => c.Key).SingleOrDefault(c => c == component) != null;
        }


        private static KeyValuePair<Component, Component>[] CloneComponents(Animator animator,
            IEnumerable<StoryObject> storyObjects)
        {
            KeyValuePair<Component, Component>[] clonedComponents = null;
            foreach (var o in storyObjects)
            {
                var newComps = o.Components.Where(c => c != animator).ToList()
                    .Select(c => new KeyValuePair<Component, Component>(c, c.Clone() as Component));

                clonedComponents = clonedComponents == null
                    ? newComps.ToArray()
                    : clonedComponents.Concat(newComps).ToArray();
            }

            return clonedComponents;
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}