using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.Interfaces;

namespace StoryMaker.Models.Builder
{
    public interface IActionBuilder
    {
        IAction Create<T>() where T : IAction;
    }
}
