using System;
using StoryMaker.DataStructure;
using System.IO;
using StoryMaker.Helpers;
using StoryMaker.Models;
using StoryMaker.ModelToDataStructConverter;

namespace StoryMaker.VersionConverter
{
    public class ProjectProvider:IProjectProvider
    {
        IConverter<ProjectDS, ProjectModel> _projectConverter;
        
        public ProjectProvider(IConverter<ProjectDS, ProjectModel> projectConverter)
        {
            _projectConverter = projectConverter;
        }
        public ProjectModel LoadProject(string path)
        {
            return _projectConverter.ConvertToModel(LoadProjectDS(path));
        }

        protected virtual ProjectDS LoadProjectDS(string path)
        {
            var dInfo = new DirectoryInfo(path);

            var files = dInfo.GetFiles("*.sps");
            if (files.Length != 1)
                throw new Exception("Project file is not unique!");

            var jsonStream = new JsonStream<ProjectDS>();
            var projectDs = jsonStream.Read($"{files[0].FullName}");

            return projectDs;
        }
    }
}
