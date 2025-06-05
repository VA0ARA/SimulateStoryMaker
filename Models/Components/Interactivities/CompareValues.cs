using System.Linq;
using StoryMaker.DataStructure;

namespace StoryMaker.Models.Components.Interactivities
{
    public class CompareValues : InterActivity
    {
        public CompareValues()
        {
            //_stateHolder = new StateHolder(this, true);
            ID = System.Guid.NewGuid().ToString();
            Parameter1=new IntVariableModel("");
            Parameter2=new IntVariableModel("");
             
        }
        public CompareValues(string id,IntVariableModel parameter1,IntVariableModel parameter2)
        {
            this.ID = id;
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
        }

        ConditionType _conditionType;

        public ConditionType ConditionType
        {
            get { return _conditionType; }

            set
            {
                if (_conditionType == value)
                    return;

                _conditionType = value;
                RaisePropertyChanged(nameof(ConditionType));
            }
        }


        private IntVariableModel _parameter1;

        public IntVariableModel Parameter1
        {
            get => _parameter1;

            set
            {
                if (_parameter1 != value)
                {
                    _parameter1 = value;
                    RaisePropertyChanged(nameof(Parameter1));
                }
            }
        }

        private IntVariableModel _parameter2;

        public IntVariableModel Parameter2
        {
            get => _parameter2;

            set
            {
                if (_parameter2 != value)
                {
                    _parameter2 = value;
                    RaisePropertyChanged(nameof(Parameter2));
                }
            }
        }

        protected override void Play()
        {
            if (!_parameter1.Constant)
            {
                _parameter1.ValueChanged += OnParameterChanged;
            }

            if (!_parameter2.Constant)
                _parameter2.ValueChanged += OnParameterChanged;
            
            base.Play();
        }

        protected override void Pause()
        {
            if (!_parameter1.Constant)
                _parameter1.ValueChanged -= OnParameterChanged;

            if (!_parameter2.Constant)
                _parameter2.ValueChanged -= OnParameterChanged;
            base.Pause();
        }

        void OnParameterChanged(object sender,int val)
        {
            CheckCondition();
        }

        void CheckCondition()
        {
            bool result = false;

            switch (ConditionType)
            {
                case ConditionType.Equal:
                    result = Parameter1.Value == Parameter2.Value;
                    break;

                case ConditionType.Greater:
                    result = Parameter1.Value > Parameter2.Value;
                    break;

                case ConditionType.Greater_Equal:
                    result = Parameter1.Value >= Parameter2.Value;
                    break;

                case ConditionType.Less:
                    result = Parameter1.Value < Parameter2.Value;
                    break;

                case ConditionType.Less_Equal:
                    result = Parameter1.Value <= Parameter2.Value;
                    break;

                case ConditionType.Not_Equal:
                    result = Parameter1.Value != Parameter2.Value;
                    break;
            }

            if (result)
                Execute();
        }

        public override TriggerType GetTriggerType()
        {
            return TriggerType.CompareVariables;
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return true;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Parameter1 != null)
                Parameter1.ValueChanged -= OnParameterChanged;
            if (Parameter2 != null)
                Parameter2.ValueChanged -= OnParameterChanged;
        }
    }
}