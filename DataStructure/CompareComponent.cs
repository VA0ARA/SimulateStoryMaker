using System;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class CompareInteractivityDS:InteractivityDS
    {
        public ConditionType ConditionType;
        public VariableDS Parameter1, Parameter2;

        public CompareInteractivityDS(VariableDS parameter1,VariableDS parameter2,ConditionType conditionType):base( TriggerType.CompareVariables)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            ConditionType = conditionType;
        }

    }
}
