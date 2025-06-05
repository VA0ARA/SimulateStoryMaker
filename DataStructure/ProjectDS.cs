using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class ProjectDS
    {
        //the projectds constructor is private to force create new instance from create method
        [JsonConstructor]
        ProjectDS()
        {

        }

        /// <summary>
        /// This static method is responsible for creating project data structure
        /// </summary>
        /// <returns></returns>
        public static ProjectDS Create()
        {
            var prj = new ProjectDS();
            prj._graph = new Graph.GraphDataStructure();
            prj.Assets = new List<IAsset>();

            return prj;
        }
        public string Name;

        [JsonProperty]
        List<IComponent<ProjectDS>> _components = new List<IComponent<ProjectDS>>();
        public List<IAsset> Assets { get; set; }// = new List<Asset>();

        [JsonIgnore]
        public SceneDS[] Scenes
        {
            get
            {
                return _components.OfType<SceneDS>().ToArray();
            }
        }

        public void AddComponent(IComponent<ProjectDS> component)
        {
            _components.Add(component);
        }

        public void AddComponents(IEnumerable<IComponent<ProjectDS>> components)
        {
            foreach (var c in components)
                AddComponent(c);
        }

        [JsonProperty]
        Graph.GraphDataStructure _graph;// = new Graph.GraphDataStructure();

        public void SetGraph(Graph.GraphDataStructure graph)
        {
            _graph = graph;
        }

        public Graph.GraphDataStructure GetGraph()
        {
            return _graph;
        }

        [JsonIgnore]
        public SceneNavigationGraph.Graph SceneNavigationGraph
        {
            get
            {
                return new SceneNavigationGraph.GraphConverter().Convert(_graph,Scenes);
            }
        }
    }
}
