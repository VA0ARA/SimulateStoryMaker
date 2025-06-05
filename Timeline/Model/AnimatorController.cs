using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using StoryMaker.Annotations;

namespace Timeline.Model
{
    public class AnimatorController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ObservableCollection<Clip> _clips = new ObservableCollection<Clip>();
        public ObservableCollection<Clip> Clips => _clips;

        private Clip _defaultClip;
        public Clip DefaultClip
        {
            get { return _defaultClip; }
            set
            {
                if (_defaultClip == value) return;
                _defaultClip = value;
                RaisePropertyChanged(nameof(DefaultClip));
            }
        }

        public void AddClip(Clip clip)
        {
            if (_clips.FirstOrDefault(c =>
                    string.Equals(c.Name, clip.Name, StringComparison.CurrentCultureIgnoreCase)) != null) return;

            _clips.Add(clip);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}