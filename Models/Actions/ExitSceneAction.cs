using StoryMaker.Helpers;
using StoryMaker.Models.Interfaces;
using System;
using StoryMaker.Models.Components;
using System.Windows;

namespace StoryMaker.Models.Actions
{
    public class ExitSceneAction : NotifyPropertyChange, IAction
    {

        public float StartTime { get; set; }
        string _exitGate;
        public string ExitGate
        {
            get
            {
                return _exitGate;
            }

            set
            {
                _exitGate = value;
                RaisePropertyChanged(nameof(ExitGate));
            }
        }

        public string ID { get; }
        public ExitSceneAction()
        {
            ID = Guid.NewGuid().ToString();
        }

        public ExitSceneAction(string id)
        {
            this.ID = id;
        }

        public async void Execute()
        {
            MessageBox.Show($"خروج از سکانس از درب : {ExitGate} ");
        }
    }
}
