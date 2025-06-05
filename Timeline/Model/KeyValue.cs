using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryMaker.Annotations;

namespace Timeline.Model
{
    /// <summary>
    /// This class stores the data in each frame
    /// </summary>
    public class KeyValue : INotifyPropertyChanged
    {
        private int _frame;

        public int Frame
        {
            get { return _frame; }
            set {
                if (_frame == value) return;
                _frame = value;
                RaisePropertyChanged(nameof(Frame));
            }
        }

        public object Val { get; set; }

        public KeyValue(int frame, object val)
        {
            Frame = frame;
            Val = val;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}