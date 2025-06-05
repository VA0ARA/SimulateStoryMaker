using Graph.Model;
using Newtonsoft.Json;
using StoryMaker.DataStructure.Graph;
using StoryMaker.ModelToDataStructConverter;

namespace StoryMaker.Graph
{
    public class DefaultGraphBuilder
    {
        private IConverter<GraphDataStructure, GraphModel> _graphConverter;
        public DefaultGraphBuilder(IConverter<GraphDataStructure, GraphModel> graphConverter)
        {
            _graphConverter = graphConverter;
        }
        public GraphModel Create()
        {
            string json = DefaultGraph.JSON;
            var graph=JsonConvert.DeserializeObject<GraphDataStructure>(
                json,new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    TypeNameHandling= TypeNameHandling.Auto
                });

            return _graphConverter.ConvertToModel(graph);
        }
    }
}