using StoryMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StoryMaker.ModelToDataStructConverter;
using StoryMaker.DataStructure;
using StoryMaker.Helpers;
using StoryMaker.Models.Software;
using helpers = Helpers;
using Newtonsoft.Json;
namespace StoryMaker.Core.Save_Load
{
    public class ImportExportScene : IImportExportScene
    {
        private IConverter<SceneDS, SceneModel> _sceneConverter;
        IAssetExport _assetExport;
        CurrentProject _currentProject;

        public ImportExportScene(IConverter<SceneDS, SceneModel> sceneConverter,IAssetExport assetExport,CurrentProject currentProject)
        {
            _sceneConverter = sceneConverter;
            _assetExport = assetExport;
            _currentProject = currentProject;
        }

        public void ExportScene(SceneModel scene,string path)
        {

            var sceneDS = _sceneConverter.ConvertToDataStruct(scene);

            //declare temp path
            string temp = Paths.DocumentTemp + "/Save";

            //destroy last temp folder if exists & create new one
            if (Directory.Exists(temp))
                Directory.Delete(temp, true);

            Directory.CreateDirectory(temp);

            //todo:collect all assets used in project
            _assetExport.Export(temp, scene.GetAssets().Select(a => a.FilePath));
            JsonStream<SceneDS> json = new JsonStream<SceneDS>();
            json.Write($"{ temp}/{scene.Name}.scn", sceneDS);
            var newSceneDS = json.Read($"{ temp}/{scene.Name}.scn");

            System.IO.Compression.ZipFile.CreateFromDirectory(temp, path);

            Directory.Delete(temp, true);
            Utils.ShowSuccessFullyMessage("سکانس با موفقیت ذخیره شد.");
        }

        public IEnumerable<AssetReplacementInfo> ImportScene(string path)
        {
            //declare temp path
            DirectoryInfo temp =new DirectoryInfo(Paths.DocumentTemp + "/Save");


            //destroy last temp folder if exists & create new one
            if (temp.Exists)
                temp.Delete(true);

            temp.Create();
            //extract file
            System.IO.Compression.ZipFile.ExtractToDirectory(path, temp.FullName);
            var files = temp.GetFiles();
            if (files.Count(f => f.Extension.ToLower() == $".{Paths.SceneExportExtension}") == 1)
            {
                FileInfo sceneFile = files.Single(f => f.Extension.ToLower() == $".{Paths.SceneExportExtension}");
                JsonStream<SceneDS> json = new JsonStream<SceneDS>();
                SceneDS scene = json.Read(sceneFile.FullName);
                var projectScenes = _currentProject.Project.Scenes;


                string newSceneName=helpers.NameSuggession.SuggestName(scene.Name,projectScenes.Select(s=>s.Name));
                scene.Name = newSceneName;

                var assets = scene.Assets;

                IEnumerable<AssetReplacementInfo> replacedAssets;
                var assetsFilename = assets.Select(a => Path.Combine(temp.FullName, AssetsController.GetFilteredLocalAssetPath(a.Path)));
                _assetExport.Export(Paths.ProjectPath,assetsFilename ,out replacedAssets);

                _currentProject.Project.AddScene(ReplaceAssets(scene,replacedAssets), false);
                return replacedAssets;
            }

            Utils.ShowSuccessFullyMessage("سکانس با موفقیت بارگزاری شد.");

            return new AssetReplacementInfo[0];
        }

        SceneDS ReplaceAssets(SceneDS scene,IEnumerable<AssetReplacementInfo> replacedAssets)
        {
            string json = JsonConvert.SerializeObject(scene, Formatting.Indented, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            });

            foreach (var asset in replacedAssets)
                json=json.Replace(asset.OldAssetName, asset.NewAssetName);

            return JsonConvert.DeserializeObject<SceneDS>(json, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
