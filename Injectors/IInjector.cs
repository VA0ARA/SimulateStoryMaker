using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
namespace StoryMaker.Injectors
{
    public interface IInjector
    {
        void Add(IUnityContainer container);
    }
}
