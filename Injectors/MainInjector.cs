using System;
using Unity;
using StoryMaker.Models.Software;
using StoryMaker.ViewModels.Components;
using StoryMaker.Models;
using StoryMaker.DataStructure;
using StoryMaker.ModelToDataStructConverter;
using System.Linq;

namespace StoryMaker.Injectors
{
    public class MainInjector
    {
        UnityContainer _container = new UnityContainer();
        public MainInjector()
        {
            if(Singleton!=null)
                throw new Exception("There's another instance of singleton!");

            Singleton = this;
            AddExternalInjectors();
        }
        
        public static MainInjector Singleton { get; private set; }
        public DropConditionViewModel DropConditionViewModel => _container.Resolve<DropConditionViewModel>();

        public CurrentProject CurrentProject => _container.Resolve<CurrentProject>();

        void AddExternalInjectors()
        {
            var assembely = GetType().Assembly;
            Type injectorType = typeof(IInjector);
            var injectors = assembely.GetTypes().Where(t => injectorType.IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var i in injectors)
            {
                var injector = (IInjector)assembely.CreateInstance(i.ToString());
                injector.Add(_container);
            }
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
