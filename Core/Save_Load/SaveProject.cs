using StoryMaker.Models;
using System.IO;
using StoryMaker.Helpers;
using StoryMaker.DataStructure;
using StoryMaker.Injectors;
using StoryMaker.ModelToDataStructConverter;
using StoryMaker.Models.Software;

namespace StoryMaker.Core.Save_Load
{
    //todo :injection cleansing
    public class SaveProject:ISaveProject
    {
        CurrentProject _currentProject;
        ProjectConverter _projectConverter;
        public SaveProject(CurrentProject currentProject,ProjectConverter projectConverter)
        {
            _currentProject = currentProject;
            _projectConverter = projectConverter;
            Utils.ShowSuccessFullyMessage("پروژه با موفقیت ذخیره شد.");
        }
        
        public virtual void Save(ProjectModel project,string path)
        {
            _currentProject.SaveScene();
            var dataStructure = _projectConverter.ConvertToDataStruct(project);

            string version = new Models.Software.SoftwareModel().Version;
            File.WriteAllText(Path.Combine(path , "Version.txt"), version);
            var jsonStream = new JsonStream<ProjectDS>();
            jsonStream.Write(Path.Combine(path, $"{project.Name}.{Paths.ProjectExtension}"), dataStructure);
            Utils.ShowSuccessFullyMessage("پروژه با موفقیت ذخیره شد.");
        }
    }
}
