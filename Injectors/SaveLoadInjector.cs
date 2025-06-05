using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using StoryMaker.DataStructure;
using StoryMaker.ModelToDataStructConverter;
using StoryMaker.Models;
using StoryMaker.Models.Software;
using StoryMaker.Core.Save_Load;

namespace StoryMaker.Injectors
{
    public class SaveLoadInjector : IInjector
    {
        public void Add(IUnityContainer container)
        {
            container.RegisterSingleton<CurrentProject>();
            container.RegisterType<IConverter<SceneDS, SceneModel>, SceneConverter>();

            container.RegisterType<ISaveProject, SaveProject>();
            container.RegisterSingleton<SaveProject>();
            container.RegisterType<ISaveProject, ExportProject>("Export");
            container.RegisterSingleton<ExportProject>();

            container.RegisterType<IAssetExport, AssetExport>();
            container.RegisterSingleton<AssetExport>();

            container.RegisterType<IImportExportScene, ImportExportScene>();
            container.RegisterSingleton<ImportExportScene>();
        }
    }
}
