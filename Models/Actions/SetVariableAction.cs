using StoryMaker.Helpers;
using StoryMaker.Models.Interfaces;
using System;
using System.Threading.Tasks;
using StoryMaker.Models.Components;

namespace StoryMaker.Models.Actions
{
    public class SetVariableAction : NotifyPropertyChange, IAction, IStop
    {
        public string ID { get; }

        public SetVariableAction()
        {
            ID = Guid.NewGuid().ToString();
        }
        public SetVariableAction(string id)
        {
            this.ID = id;
        }
        private float _startTime;
        public float StartTime
        {
            get => _startTime;
            set
            {
                if(_startTime == value) return;

                _startTime = value;
                RaisePropertyChanged(nameof(StartTime));
            }
        }

        private IntVariableModel _parameter1;

        public IntVariableModel Parameter1
        {
            get { return _parameter1; }

            set
            {
                _parameter1 = value;
                RaisePropertyChanged(nameof(Parameter1));
            }
        }

        private IntVariableModel _parameter2;

        public IntVariableModel Parameter2
        {
            get { return _parameter2; }

            set
            {
                _parameter2 = value;
                RaisePropertyChanged(nameof(Parameter2));
            }
        }

        private bool _increase;

        public bool Increase
        {
            get { return _increase; }

            set
            {
                _increase = value;
                RaisePropertyChanged(nameof(Increase));
            }
        }

        public async void Execute()
        {
            await Task.Delay(TimeSpan.FromSeconds(StartTime));
            Parameter1.Value = Increase ? Parameter1.Value + Parameter2.Value : Parameter2.Value;
        }

        public void Stop()
        {
            Parameter1.Value = Parameter1.DefaultValue;
            Parameter2.Value = Parameter2.DefaultValue;
        }
    }
}