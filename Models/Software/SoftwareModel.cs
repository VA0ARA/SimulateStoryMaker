using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SemVersion;
using StoryMaker.Helpers;

namespace StoryMaker.Models.Software
{
    public class SoftwareModel : NotifyPropertyChange
    {
        public string Version
        {
            get
            {
                var semanticVersion = new SemanticVersion(0, 0, 1, "beta", "1");
                return semanticVersion.ToString();
            }
        }

        private EditorModel _editorModel;

        public EditorModel EditorModel
        {
            get => _editorModel;
            set
            {
                if (_editorModel != null && _editorModel.Equals(value)) return;
                _editorModel = value;
                RaisePropertyChanged(nameof(EditorModel));
            }
        }

        /// <summary>
        /// list of project name , with project path and last opened time
        /// KeyValuePair : Key is project name , Value is project path
        /// </summary>
        public ObservableCollection<KeyValuePair<KeyValuePair<string, string>, DateTime>> RecentProject { get; set; } =
            new ObservableCollection<KeyValuePair<KeyValuePair<string, string>, DateTime>>();

        private Setting _setting = new Setting();

        public Setting Setting
        {
            get { return _setting; }
            set
            {
                if (_setting == value) return;
                _setting = value;
                RaisePropertyChanged(nameof(Setting));
            }
        }
    }
}