using StoryMaker.Models;
using Unity;
using StoryMaker.Models.Components;
using StoryMaker.Models.Components.Interactivities;
using StoryMaker.Models.Interfaces;
using graph=Graph.Model;
using StoryMaker.ModelToDataStructConverter.ActionConverters;
using StoryMaker.ModelToDataStructConverter.InteractivityConverter;
using actions=StoryMaker.DataStructure.Actions;
using StoryMaker.Models.Actions;
using StoryMaker.DataStructure;
using StoryMaker.DataStructure.Actions;
using StoryMaker.ModelToDataStructConverter;
using StoryMaker.ModelToDataStructConverter.ComponentFeed;

namespace StoryMaker.Injectors
{
    public class ConverterInjector : IInjector
    {
        public void Add(IUnityContainer container)
        {
            container.RegisterType<IConverter<ProjectDS, ProjectModel>, ProjectConverter>();
            container.RegisterType<IConverter<SceneDS, SceneModel>, SceneConverter>();
            container.RegisterType<IConverter<VariableDS, IntVariableModel>, VariableConverter>();
            container.RegisterType<IConverter<VariableDS, IntVariableModel>, ReUsedVariableConverter>("ReUse");
            container.RegisterType<IConverter<ElementDS, StoryObject>, ElementConverter>();
            container.RegisterType<IConverter<DataStructure.Graph.GraphDataStructure, graph.GraphModel>, GraphConverter>();
            container.RegisterType<IConverter<DataStructure.Assets.IAsset, Models.AssetModels.AssetModel>, AssetConverter>();

            //register component feeds
            container.RegisterType<IComponentFeed<Component>, MainComponentFeed>();
            container.RegisterType<IComponentFeed<Camera>, CameraFeed>();
            container.RegisterType<IComponentFeed<Animator>, AnimatorFeed>();
            container.RegisterType<IComponentFeed<InterActivity>, InteractivityFeed>();
            container.RegisterType<IComponentFeed<Transform>, TransformFeed>();
            container.RegisterType<IComponentFeed<ImageRenderer>, ImageFeed>();
            container.RegisterType<IComponentFeed<AudioSource>, AudioSourceFeed>();

            //register animator parts
            container.RegisterType<IConverter<ClipDS, Timeline.Model.Clip>, ClipConverter>();
            container.RegisterType<IConverter<ChannelDS, Timeline.Model.Channel>, ChannelConverter>();
            container.RegisterType<IConverter<KeyValueDS, Timeline.Model.KeyValue>, KeyConverter>();
            container.RegisterType<IDynamicConverter, DynamicObjectConverter>();
            container.RegisterType<ITypeMapper, TypeMapper>();

            //register compare condition converter
            container.RegisterType<IConverter<CompareInteractivityDS, CompareValues>, CompareConverter>();

            //register drop condition interactivity converter
            container.RegisterType<IConverter<DataStructure.DropCondition,
                    Models.Components.Interactivities.DropCondition>, DropConditionConverter>();

            //register timer interactivity converter
            container.RegisterType<IConverter<TimerInteractivityDS, Timer>, TimerConverter>();
            container.RegisterSingleton<TimerConverter>();

            //register action converter
            container.RegisterType<IConverter<DataStructure.IComponent<InteractivityDS>, IAction>, ActionConverter>();

            container.RegisterType<
                IConverter<actions.ChangeImageDS, ChangeImageAction>,
                ChangeImageConverter>();
            container.RegisterType<IConverter<ExitSceneDS, ExitSceneAction>, ExitSceneConverter>();
            container.RegisterType<IConverter<actions.PlayAnimationActionDS, PlayAnimationAction>,
                PlayAnimationConverter>();
            container.RegisterType<IConverter<PlaySoundDS, PlaySoundAction>, PlaySoundConverter>();
            container.RegisterType<IConverter<SetVariableDS, SetVariableAction>, SetVariableConverter>();
            container.RegisterType<IConverter<actions.SetVisibilityDS, SetVisibilityAction>, SetVisibilityConverter>();


            //define singleton
            container.RegisterSingleton<SceneConverter>();
            container.RegisterSingleton<ElementConverter>();
        }
    }
}