using StoryMaker.DataStructure;
using System.IO;
using StoryMaker.Helpers;
using StoryMaker.Models;
using StoryMaker.ModelToDataStructConverter;

namespace StoryMaker.VersionConverter
{
    public class JSONProjectProvider : IProjectProvider
    {
        private IConverter<ProjectDS, ProjectModel> _projectConverter;

        public JSONProjectProvider(IConverter<ProjectDS, ProjectModel> projectConverter)
        {
            _projectConverter = projectConverter;
        }
        public ProjectModel LoadProject(string path)
        {
            return _projectConverter.ConvertToModel(LoadProjectDS(path));
        }

        ProjectDS LoadProjectDS(string path)
        {
            var dInfo = new DirectoryInfo(path);

            var files = dInfo.GetFiles("*.JSONPRJ");

            var binaryStreamer = new BinaryStream<ProjectDS>();
            var projectDs =new JsonStream<ProjectDS>().Read($"{files[0].FullName}");

            return projectDs;
        }

    }
}
