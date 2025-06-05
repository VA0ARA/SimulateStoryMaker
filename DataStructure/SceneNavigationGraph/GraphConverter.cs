using System.Collections.Generic;
using System.Linq;

namespace StoryMaker.DataStructure.SceneNavigationGraph
{
    public class GraphConverter
    {
        public virtual SceneNavigationGraph.Graph Convert(StoryMaker.DataStructure.Graph.GraphDataStructure graph,IEnumerable<SceneDS> scenes)
        {
            var startCon = graph.NodeRelations.Single(c => c.LeftNode.Type.Equals("Start"));
            string rootName = startCon.RightNode.Name;
            SceneDS root = scenes.Single(s => s.Name.Equals(rootName));
            var newGraph = new SceneNavigationGraph.Graph(root);
            
            //add graph nodes(scenes & scene gates)
            foreach (var s in scenes)
                AddScene(newGraph, s);

            //add connections
            foreach (var con in graph.NodeRelations)
                if(!con.LeftNode.Type.Equals("Start"))
                    Connect(newGraph, scenes, con);

            return newGraph;
        }

        void Connect(Graph graph,IEnumerable<SceneDS> scenes,StoryMaker.DataStructure.Graph.NodeRelationDataStructure connection)
        {
            SceneDS scene1 = scenes.Single(s => s.Name.Equals(connection.LeftNode.Name)),
                scene2 = scenes.Single(s => s.Name.Equals(connection.RightNode.Name));

            SceneGate.SceneGateDS exitGate = scene1.ExitGates.Single(g => g.GateName.Equals(connection.LeftGate.Key)),
                startGate = scene2.StartGates.Single(g => g.GateName.Equals(connection.RightGate.Key));

            graph.Connect(exitGate, startGate);
        }

        void AddScene(SceneNavigationGraph.Graph graph, SceneDS scene)
        {
            graph.AddNode(scene);
            foreach (var g in scene.ExitGates)
            {
                graph.AddNode(g);
                graph.Connect(scene, g);
            }

            foreach (var g in scene.StartGates)
            {
                graph.AddNode(g);
                graph.Connect(g, scene);
            }

        }
    }
}
