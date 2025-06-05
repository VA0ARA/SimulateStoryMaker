using System;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class VariableDS:IComponent<SceneDS>
    {
        public string Name,ID;
        public bool IsConstant;
        public int DefaultValue;
    }
}
