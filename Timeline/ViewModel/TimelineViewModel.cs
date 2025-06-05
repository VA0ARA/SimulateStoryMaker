using System.Windows.Data;
using Timeline.Model;
using System.Windows;
using System;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using StoryMaker.Helpers;

namespace Timeline.ViewModel
{
    public class TimelineViewModel : INotifyPropertyChanged
    {
        #region Parameters

        public CommandDelegate<Channel> CaptureCommandDelegate;

        public event PropertyChangedEventHandler PropertyChanged;

        // public event Action<RecordStatus> OnPlayStatusChanged;

        // private readonly PortableLoopEngine _loopEngine;

        // private RecordStatus _recordStatus = RecordStatus.Pause;
        //
        // public RecordStatus RecordStatus
        // {
        //     get { return _recordStatus; }
        //     set
        //     {
        //         if (_recordStatus.Equals(value)) return;
        //         _recordStatus = value;
        //
        //         switch (value)
        //         {
        //             case RecordStatus.Play:
        //                 _loopEngine.StartEngine(new List<object>() {this});
        //                 break;
        //             case RecordStatus.Pause:
        //             case RecordStatus.Stop:
        //             case RecordStatus.Record:
        //                 _loopEngine.StopEngine();
        //                 break;
        //         }
        //
        //         OnPlayStatusChanged?.Invoke(_recordStatus);
        //         RaisePropertyChanged(nameof(RecordStatus));
        //     }
        // }
        //
        // private bool _previewMode;
        //
        // public bool Preview
        // {
        //     get { return _previewMode; }
        //     set
        //     {
        //         if (_previewMode == value) return;
        //         _previewMode = value;
        //
        //         if (_previewMode == false)
        //         {
        //             RecordStatus = RecordStatus.Stop;
        //
        //             UnTrack();
        //             SetComponentsToDefault();
        //         }
        //         else
        //         {
        //             Track();
        //         }
        //
        //         RaisePropertyChanged(nameof(Preview));
        //     }
        // }

        // private int _framePerSecond = 30;
        //
        // public int FramePerSecond
        // {
        //     get { return _framePerSecond; }
        //     set
        //     {
        //         if (_framePerSecond.Equals(value)) return;
        //         _framePerSecond = value;
        //         RaisePropertyChanged(nameof(FramePerSecond));
        //     }
        // }

        // private float _startFrame = 0;
        //
        // public float StartFrame
        // {
        //     get { return _startFrame; }
        //     set
        //     {
        //         if (_startFrame.Equals(value)) return;
        //
        //         _startFrame = value;
        //         FrameChanged(_startFrame);
        //         RaisePropertyChanged(nameof(StartFrame));
        //     }
        // }
        //
        // private float _endFrame = 3000;
        //
        // public float EndFrame
        // {
        //     get { return _endFrame; }
        //     set
        //     {
        //         if (_endFrame.Equals(value)) return;
        //
        //         _endFrame = value;
        //         FrameChanged(_endFrame);
        //         RaisePropertyChanged(nameof(EndFrame));
        //     }
        // }


        // public int CurrentFrame => (int) (_currentTime * FramePerSecond);
        //
        // private float _currentTime = 0;
        //
        // public float CurrentTime
        // {
        //     get { return _currentTime; }
        //     set
        //     {
        //         if (_currentTime.Equals(value)) return;
        //         _currentTime = value;
        //         RaisePropertyChanged(nameof(CurrentTime));
        //
        //         FrameChanged(CurrentFrame);
        //         RaisePropertyChanged(nameof(CurrentFrame));
        //     }
        // }

        // private Animator _animator;
        // public Animator Animator => _animator;
        //
        // private Clip _recordingClip;
        // public Clip RecordingClip => _recordingClip;

        // private Map<string, StoryObject> _objectsMap;
        //
        // private KeyValuePair<Component, Component>[] _clonedComponent;

        #endregion

        #region Constructors

        public TimelineViewModel()
        {
            CaptureCommandDelegate = new CommandDelegate<Channel>(Capture_CommandExecute, Capture_CanExecute);
            // _loopEngine = new PortableLoopEngine();
        }

        private bool Capture_CanExecute(Channel channel)
        {
            return true;
        }

        private void Capture_CommandExecute(Channel channel)
        {
            //Animator.CaptureProperty(channel.ObjectAddress, channel.MemberName, channel.Type);
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        // private void Update()
        // {
        //     if (RecordStatus != RecordStatus.Play) return;
        //
        //     if (CurrentFrame >= EndFrame || CurrentFrame < StartFrame)
        //     {
        //         CurrentTime = StartFrame / FramePerSecond;
        //     }
        //     else
        //     {
        //         CurrentTime += _loopEngine.DeltaTime;
        //     }
        // }

        private void RaisePropertyChanged(string propertyName)
        {
            var handle = PropertyChanged;
            handle?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region RecordMethods

        // public void ChangeAnimator(Animator animator)
        // {
        // }

        // private void Track()
        // {
        //     _objectsMap = AnimationUtility.GetObjectMap(_animator.StoryObject);
        //
        //     _clonedComponent = CloneComponents(_animator, _objectsMap.Set2);
        //
        //     foreach (var storyObject in _objectsMap.Set2)
        //     {
        //         storyObject.PropertyChanged += StoryObject_PropertyChanged;
        //     }
        //
        //     for (var i = 0; i < _clonedComponent.Length; i++)
        //     {
        //         _clonedComponent[i].Key.PropertyChanged += Component_OnPropertyChanged;
        //     }
        //
        //     // foreach (var o in _objectsMap.Backwards)
        //     // {
        //     //     var components = o.Key.Components.ToList();
        //     //
        //     //     foreach (var component in components)
        //     //     {
        //     //         component.PropertyChanged += Component_OnPropertyChanged;
        //     //
        //     //         _trackingComponents.Add(component);
        //     //     }
        //     //
        //     //     o.Key.PropertyChanged += StoryObject_PropertyChanged;
        //     //     o.Key.transform.PropertyChanged += StoryObject_PropertyChanged;
        //     // }
        // }
        //
        // private void UnTrack()
        // {
        //     // foreach (var component in _trackingComponents.ToList())
        //     // {
        //     //     component.PropertyChanged -= Component_OnPropertyChanged;
        //     //
        //     //     if (component.StoryObject != null)
        //     //     {
        //     //         component.StoryObject.PropertyChanged -= StoryObject_PropertyChanged;
        //     //         component.StoryObject.transform.PropertyChanged -= StoryObject_PropertyChanged;
        //     //     }
        //     //
        //     //     _trackingComponents.Remove(component);
        //     // }
        // }


        // private static KeyValuePair<Component, Component>[] CloneComponents(Animator animator, IEnumerable<StoryObject> storyObjects)
        // {
        //     KeyValuePair<Component, Component>[] clonedComponents = null;
        //     foreach (var o in storyObjects)
        //     {
        //         var newComps = o.Components.Where(c => c != animator).ToList()
        //             .Select(c => new KeyValuePair<Component, Component>(c, c.Clone() as Component));
        //
        //         clonedComponents = clonedComponents == null
        //             ? newComps.ToArray()
        //             : clonedComponents.Concat(newComps).ToArray();
        //     }
        //
        //     return clonedComponents;
        // }

        // private void SetComponentsToDefault()
        // {
        //     for (var i = 0; i < _clonedComponent.Length; i++)
        //     {
        //         _clonedComponent[i].Key.Paste(_clonedComponent[i].Value);
        //     }
        // }

        // public void AddNewClip(string name)
        // {
        //     if (_animator.AnimatorController.Clips.SingleOrDefault(c => c.Name == name) != null) return;
        //
        //     _animator.AnimatorController.Clips.Add(new Clip(name));
        // }
        //
        // public void CaptureProperty(int frame, string objectAddress, string memberName, Type type)
        // {
        //     var channel = _recordingClip.GetChannel(objectAddress, memberName, type);
        //
        //     if (!_objectsMap.Contains(channel.ObjectAddress)) return;
        //     var component = _objectsMap[channel.ObjectAddress].Components.FirstOrDefault(c => c.GetType() == type);
        //
        //     _recordingClip.Capture(channel, frame, type.GetProperty(memberName)?.GetValue(component), true);
        // }
        //
        // public void FrameChanged(float frame)
        // {
        //     if (!Preview)
        //         Preview = true;
        //     if (_recordingClip == null) return;
        //     foreach (var channel in _recordingClip.Channels)
        //     {
        //         if (!_objectsMap.Contains(channel.ObjectAddress)) continue;
        //         var obj = _objectsMap[channel.ObjectAddress];
        //
        //         if (_animator.ApplyRootMotion && obj == _animator.StoryObject) continue;
        //
        //         IValueAtTime valueAtTime = channel;
        //         var value = valueAtTime.GetValue(frame);
        //
        //         var component = obj.GetComponentByType(channel.Type);
        //
        //         var property = component?.GetType().GetProperty(channel.MemberName);
        //
        //         property?.SetValue(component, value);
        //     }
        // }

        #endregion

        // private void StoryObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     if (e.PropertyName == nameof(StoryObject.Name) || 
        //         e.PropertyName == nameof(StoryObject.transform.Children) ||
        //         e.PropertyName == nameof(StoryObject.Components))
        //     {
        //         if (!Preview) return;
        //         UnTrack();
        //         Track();
        //     }
        // }
        //
        // private void Component_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     // if record status is on playing , must not raise property changed
        //     // this is optimization trick
        //     if (RecordStatus != RecordStatus.Record) return;
        //     // if (CanRecord?.Invoke() == false) return;
        //
        //     if (_recordingClip == null) return;
        //
        //     if (!(sender is Component component) || component == _animator) return;
        //
        //     var componentType = component.GetType();
        //
        //     if (componentType.GetCustomAttribute(typeof(NonRecordable)) != null) return;
        //
        //     var property = componentType.GetProperty(e.PropertyName);
        //     if (property == null || !property.CanWrite) return;
        //     if (property.GetCustomAttribute(typeof(NonRecordable)) != null) return;
        //
        //     var objectAddress = _objectsMap[component.StoryObject];
        //
        //     CaptureProperty(objectAddress, e.PropertyName, componentType);
        // }
        //
        // public void CaptureProperty(string objectAddress, string memberName, Type type)
        // {
        //     var channel = _recordingClip.GetChannel(objectAddress, memberName, type);
        //
        //     if (!_objectsMap.Contains(channel.ObjectAddress)) return;
        //     var component = _objectsMap[channel.ObjectAddress].Components.FirstOrDefault(c => c.GetType() == type);
        //
        //     _recordingClip.Capture(channel, CurrentFrame, type.GetProperty(memberName)?.GetValue(component), true);
        // }
    }

    #region Converters

    public class ObjectAddressToMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objectAddress = (string) value ?? "";
            int.TryParse(parameter?.ToString(), out var multiple);

            var childIndex = objectAddress.Count(o => o == '/');

            return new Thickness(childIndex * multiple, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RecordToggleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (RecordStatus) value;
            return flag == RecordStatus.Record;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? RecordStatus.Record : RecordStatus.Pause;
        }
    }

    public class FrameToMarginConverter : IMultiValueConverter
    {
        private double lastColumnWidth = 20;
        private double lastZoom = 1;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double frame;
            double.TryParse(System.Convert.ToString(values[0]), out frame);
            var zoom = lastZoom = (float) values[1];
            var columnWidth = lastColumnWidth = (double) values[2];
            var left = frame * (columnWidth * zoom);
            return new Thickness(left - 1, 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var margin = (Thickness) value;

            var left = margin.Left + 1;

            var currentFrameString = left / lastColumnWidth;

            var currentZoom = left / (lastColumnWidth / lastZoom);

            object[] objects = new object[]
            {
                currentFrameString,
                currentZoom
            };

            return objects;
        }
    }

    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var par = parameter as string;

            var isNot = par == "!";

            return isNot ? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KeyMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var columnWidth = System.Convert.ToDouble(values[0]);
            var zoom = System.Convert.ToDouble(values[1]);
            var keyIndex = System.Convert.ToDouble(values[2]);
            var btnKeyWidth = System.Convert.ToDouble(values[3]);

            return new Thickness(columnWidth * zoom * keyIndex - btnKeyWidth / 2, 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimelineWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var columnWith = System.Convert.ToInt32(values[2]);
            var currentFrame = System.Convert.ToInt32(values[1]);

            if (values[0] == DependencyProperty.UnsetValue) return currentFrame * columnWith;
            var lastKey = System.Convert.ToInt32(values[0]);


            var lastKeyWidth = lastKey * columnWith;
            var currentFrameWidth = currentFrame * columnWith;
            return (double) Math.Max(lastKeyWidth, currentFrameWidth) + columnWith;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HorizontalGridLineViewportConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var viewPortWidth = System.Convert.ToDouble(values[0]);
            var leftPixel = System.Convert.ToDouble(values[1]);
            var rightPixel = leftPixel + viewPortWidth;

            var viewport = new Vector(leftPixel, rightPixel);

            return viewport;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}