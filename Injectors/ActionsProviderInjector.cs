using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using StoryMaker.Models.Builder;

namespace StoryMaker.Injectors
{
    public class ActionsProviderInjector : IInjector
    {
        public void Add(IUnityContainer container)
        {
            container.RegisterType<IActionProvider, ProvideActionFromList>();
            container.RegisterSingleton<ProvideActionFromList>();

            container.RegisterType<IActionBuilder, ActionBuilder>();
            container.RegisterSingleton<ActionBuilder>();

        }
    }
}
