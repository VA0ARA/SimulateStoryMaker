using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models;
using StoryMaker.Models.Software;
using StoryMaker.ModelToDataStructConverter;
using System.IO;
using StoryMaker.DataStructure;
using Graph.Model;
using Unity;
using StoryMaker.Helpers;

namespace StoryMaker.Core.Save_Load
{
    public class ExportProject:SaveProject
    {
        const string _saveFolder = "Save";
        IAssetExport _assetExport;
        IConverter<DataStructure.Graph.GraphDataStructure, GraphModel> _graphDS_ModelConverter;
        DataStructure.SceneNavigationGraph.GraphConverter _graphConverter;
        public ExportProject(CurrentProject currentProject,ProjectConverter projectConverter) : base(currentProject, projectConverter) { }
        [InjectionMethod]
        public void DependencyInjectionMethod(IAssetExport assetExport, IConverter<DataStructure.Graph.GraphDataStructure, GraphModel> graphDS_ModelConverter, DataStructure.SceneNavigationGraph.GraphConverter graphConverter)
        {
            _assetExport = assetExport;
            _graphConverter = graphConverter;
            _graphDS_ModelConverter = graphDS_ModelConverter;
        }

        public override void Save(ProjectModel project, string path)
        {
            DirectoryInfo temp = new DirectoryInfo(Path.Combine(Paths.DocumentTemp, _saveFolder));
            if (temp.Exists)
                temp.Delete(true);

            temp.Create();

            base.Save(project, temp.FullName);
            CopyAssets(project,temp.FullName);

            //Deliver
            if (File.Exists(path))
                File.Delete(path);

            System.IO.Compression.ZipFile.CreateFromDirectory(temp.FullName, path);
            Utils.ShowSuccessFullyMessage("پروژه با موفقیت ذخیره شد.");
        }

        void CopyAssets(ProjectModel project,string path)
        {
            var assets = GetScenes(project).SelectMany(s => s.Assets);
            _assetExport.Export(path, assets.Where(a=>a!=null).Select(a => a.Path));
        }

        //this part has little problem uncomment all to reactive it
        IEnumerable<SceneDS> GetScenes(ProjectModel project)
        {
            List<SceneDS> usedScenes = new List<SceneDS>();
            try
            {
                var graphDS = _graphDS_ModelConverter.ConvertToDataStruct(project.GraphModel);
                var simpleGraph = _graphConverter.Convert(graphDS, project.Scenes);

                usedScenes.Add(simpleGraph.Root);

                int i = 0;
                while (i < usedScenes.Count)
                {
                    var exitScenes = GetExitScenes(simpleGraph, usedScenes[i]);
                    foreach (var s in exitScenes)
                        if (s != null && !usedScenes.Contains(s))
                            usedScenes.Add(s);
                    i++;
                }


            }

            catch
            {
                return project.Scenes;
            }


            return usedScenes;
        }

        IEnumerable<SceneDS> GetExitScenes(DataStructure.SceneNavigationGraph.Graph graph,SceneDS scene)
        {
            return scene.ExitGates.Select(g => graph.GetNextScene(scene, g.GateName));
        }
    }
}
