using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoryMaker.Annotations;

namespace StoryMaker.Models.Software
{
    public class SaveSetting : ISetting, INotifyPropertyChanged
    {
        public string Name
        {
            get => "Save";
        }

        private bool _autoSave = true;
        public bool AutoSave
        {
            get { return _autoSave;}
            set
            {
                if(_autoSave == value) return;
                _autoSave = value;
                RaisePropertyChanged(nameof(AutoSave));
            }
        }

        private int _autoSaveTimer = 5;
        public int AutoSaveTimer
        {
            get { return _autoSaveTimer; }
            set
            {
                if (_autoSaveTimer == value) return;
                _autoSaveTimer = value;
                RaisePropertyChanged(nameof(AutoSaveTimer));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}