using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using StoryMaker.Annotations;
using StoryMaker.Helpers;
using StoryMaker.Models.Input.Enum;
using StoryMaker.Models.Input.Interface;
using Timeline.Attribute;

namespace StoryMaker.Models.Components
{
    public abstract class Component : IMouseDown, IMouseDrag, IMouseUp, Interfaces.ICloneable<Component>,
        INotifyPropertyChanged
    {

        private StoryObject _storyObject;

        private string _id;
        public string ID { get; protected set; }
        public StoryObject StoryObject
        {
            get { return _storyObject; }
        }

        // ~Component()
        // {
        //     Debug.WriteLine($"{nameof(GetType)} destroyed of {StoryObject?.Name} object");
        // }

        public virtual void MouseDown()
        {
        }

        public virtual void MouseDrag()
        {
        }

        public virtual void MouseUp()
        {
        }

        public void RaiseEvent(MouseEventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.MouseDown:
                    MouseDown();
                    break;
                case MouseEventType.MouseDrag:
                    MouseDrag();
                    break;
                case MouseEventType.MouseUp:
                    MouseUp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }

        public virtual bool CanAdd(StoryObject storyObject)
        {
            return true;
        }

        public virtual bool CanRemove(StoryObject storyObject)
        {
            return true;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        //public abstract object Clone();

        public void Paste(Component component)
        {
            var type = component.GetType();
            if (type != GetType()) return;

            UpdateForType(component, this);
            this.ID = component.ID;
        }

        private static void UpdateForType(Component source, Component destination)
        {
            var type = source.GetType();

            var myObjectProperty = type
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                // .Where(p => p.GetCustomAttribute(typeof(NonRecordable)) == null)
                // .Where(p => p.GetCustomAttribute(typeof(NonSerializedAttribute)) == null)
                .Where(p => p.CanWrite && p.GetSetMethod() != null);

            foreach (var propertyInfo in myObjectProperty)
            {
                propertyInfo.SetValue(destination, propertyInfo.GetValue(source));
            }

            //Todo Care for copy paste
            // var myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            //     .Where(p => p.GetCustomAttribute(typeof(NonSerializedAttribute)) == null);
            //
            // foreach (var fi in myObjectFields)
            // {
            //     fi.SetValue(destination, fi.GetValue(source));
            // }
        }

        public static void AssignStoryObject(Component component, StoryObject storyObject)
        {
            component._storyObject = storyObject;
            component.StoryObjectAssigned();
        }

        public static void ReleaseStoryObject(Component component, StoryObject storyObject)
        {
            if (component._storyObject != storyObject) return;
            component.StoryObjectReleased();
            component._storyObject = null;
        }

        /// <summary>
        /// This method calls when component attached to story object
        /// </summary>
        protected virtual void StoryObjectAssigned()
        {
        }

        /// <summary>
        /// This method calls when a component detached from story object
        /// </summary>
        protected virtual void StoryObjectReleased()
        {
        }


        #region INotifyPropertyChanged

        private readonly List<PropertyChangedEventHandler> _propertyChangedList =
            new List<PropertyChangedEventHandler>();

        public event EventHandler OnStateChanging;
        public event EventHandler OnStateChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (_propertyChangedList.Contains(value)) return;
                _propertyChangedList.Add(value);
            }
            remove
            {
                if (!_propertyChangedList.Contains(value)) return;
                _propertyChangedList.Remove(value);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            // _propertyChangedList.ForEach(p => p?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            for (var i = 0; i < _propertyChangedList.Count; i++)
            {
                _propertyChangedList[i]?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public void ChangeState(PropertyInfo property, object value)
        {
            OnStateChanging?.Invoke(this, null);
            property.SetValue(this, value);
            OnStateChanged?.Invoke(this, null);
        }
    }
}