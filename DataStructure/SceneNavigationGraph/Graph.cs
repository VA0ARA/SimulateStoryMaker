
using System.Linq;

using StoryMaker.DataStructure.Graph;


namespace StoryMaker.DataStructure.SceneNavigationGraph
{
    public class Graph:GenericGraph<SceneGate.ISceneNode>
    {
        public SceneDS Root { get; }
        public Graph(SceneDS root):base()
        {
            Root = root;
        }

        public SceneDS GetNextScene(SceneDS scene,string gateName)
        {
            var gate = scene.ExitGates.Single(e => (e.GateName == gateName));
            var nextgateNode = Info.GetConnectedNodes(gate).OfType<SceneGate.SceneGateDS>().FirstOrDefault();
            if (nextgateNode == null) return null;
            var nextScene =Info.GetConnectedNodes(nextgateNode).OfType<SceneDS>().FirstOrDefault();
            return nextScene;
        }
    }
}
