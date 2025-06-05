using System;
using System.Linq;
using System.ComponentModel;
using  StoryMaker.ModelToDataStructConverter;
using  StoryMaker.DataStructure;


namespace StoryMaker.Models.Software
{
    public class CurrentProject:INotifyPropertyChanged
    {
        public CurrentProject(IConverter<SceneDS, SceneModel> sceneConverter)
        {
            _sceneConverter = sceneConverter;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IConverter<SceneDS, SceneModel> _sceneConverter;
        ProjectModel _project;
        public ProjectModel Project
        {
            get => _project;
            set
            {
                if(_project!=value)
                {
                    _project = value;
                    ChangeScene(Project.Scenes.First());
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Project)));
                    
                }
            }
        }
        

        SceneModel _currentScene;
        public SceneModel CurrentScene
        {
            get => _currentScene;
            private set
            {
                if(_currentScene!=value)
                {
                    _currentScene = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScene)));
                }
            }
        }
        
        public void SaveScene()
        {
            if (CurrentScene == null)
                return;
            Project.AddScene(_sceneConverter.ConvertToDataStruct(CurrentScene),true);
        }
        
        public void ChangeScene(SceneDS sceneDs)
        {
            if (sceneDs == null) return;

            SaveScene();
            CurrentScene?.Dispose();
            CurrentScene = _sceneConverter.ConvertToModel(sceneDs);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void ChangeSceneWithoutSave(SceneDS sceneDs)
        {
            if (sceneDs == null) return;

            //SaveScene();
            CurrentScene?.Dispose();
            CurrentScene = _sceneConverter.ConvertToModel(sceneDs);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
