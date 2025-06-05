using System;

namespace StoryMaker.DataStructure.SceneGate
{
    [Serializable]
    public class SceneGateDS:IComponent<SceneDS>,ISceneNode
    {
        public const string DefaultInputGateName = "ورودی", DefaultOutputGateName = "اتمام سکانس";
        public string GateName;
        public GateType GateType;

        public SceneGateDS(string gateName,GateType gateType)
        {
            GateName = gateName;
            GateType = gateType;
        }
    }
}
